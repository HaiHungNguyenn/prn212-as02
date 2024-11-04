using BusinessObjects;
using Repositories;
using Repositories.IRepository;
using System.Diagnostics;
using System.Windows;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for CreateBookingReservationWindow.xaml
    /// </summary>
    public partial class CreateBookingReservationWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInformationService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;

        private Action _delegate;

        public void DelegateAction(Action action)
        {
            _delegate += action;
        }

        public CreateBookingReservationWindow(IRoomInformationService roomInformationService, ICustomerService
            customerService, IBookingReservationService bookingReservationRepo, IBookingDetailService bookingDetailService)
        {
            InitializeComponent();
            _customerService = customerService;
            _roomInformationService = roomInformationService;
            _bookingReservationService = bookingReservationRepo;
            _bookingDetailService = bookingDetailService;
            PopulateData();
        }

        private void PopulateData()
        {
            cbCustomers.ItemsSource = _customerService.GetAll().ToList();
            cbRooms.ItemsSource = _roomInformationService.GetAll().ToList();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateBookingReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var isValidPrice = decimal.TryParse(txtTotalPrice.Text, out var totalPrice);
                var isValidCustomerId = int.TryParse(cbCustomers.SelectedValue.ToString(), out var customerId);
                var isValidRoomId = int.TryParse(cbRooms.SelectedValue.ToString(), out var roomId);

                if (!isValidCustomerId)
                    throw new Exception("Customer id is invalid");

                if (!isValidRoomId)
                    throw new Exception("Room id is invalid");

                if (!isValidPrice)
                    throw new Exception("Price is invalid");

                if (totalPrice <=0 ) {
                    throw new Exception("Price must greater than or equal to 0.");
                }

                var startDate = dpStartDate.SelectedDate ?? throw new Exception("Start date is invalid");
                var endDate = dpEndDate.SelectedDate ?? throw new Exception("End date is invalid");

                if (startDate < DateTime.Now || endDate < DateTime.Now) {
                    throw new Exception("The reservation date must be in future.");
                }

                if (startDate > endDate)
                    throw new Exception("Start date must be before end date");

                var selectedRoom = _roomInformationService.GetById(x => x.RoomId == roomId) ?? throw new Exception("Room does not exist");

                foreach (var bookingDetail in selectedRoom.BookingDetails)
                {
                    if (bookingDetail.StartDate <= startDate && bookingDetail.EndDate >= startDate)
                        throw new Exception("Room is already booked in this period");
                    if (bookingDetail.StartDate <= endDate && bookingDetail.EndDate >= endDate)
                        throw new Exception("Room is already booked in this period");
                }

                var customer = _customerService.GetById(x => x.CustomerId == customerId) ?? throw new Exception("Customer does not exist");

                var bookingReservation = new BookingReservation
                {
                    BookingReservationId = GenerateBookingReservationId(),
                    BookingDate = DateTime.Now,
                    CustomerId = customerId,
                    TotalPrice = totalPrice,
                    BookingStatus = 1
                };

                var newBookingDetail = new BookingDetail
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    ActualPrice = totalPrice,
                    RoomId = roomId,
                    BookingReservationId = bookingReservation.BookingReservationId
                };

                _bookingReservationService.Create(bookingReservation);
                _bookingDetailService.Create(newBookingDetail);
                MessageBox.Show("Booking reservation created successfully");
                _delegate.Invoke();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int GenerateBookingReservationId()
        {
            int randomInt;
            do
            {
                randomInt = new Random().Next();
            } while (_bookingReservationService.GetAll().Where(x => x.BookingReservationId == randomInt).Any());

            return randomInt;
        }
    }
}
