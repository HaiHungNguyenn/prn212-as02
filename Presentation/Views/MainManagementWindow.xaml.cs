using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for MainManagementWindow.xaml
    /// </summary>
    public partial class MainManagementWindow : Window
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomInformationService _roomInformationService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IServiceProvider _serviceProvider;

        public MainManagementWindow(ICustomerService customerService, IRoomInformationService roomInformationService, IBookingReservationService bookingReservationService, IBookingDetailService bookingDetailService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            try
            {
                _serviceProvider = serviceProvider; 
                _customerService = customerService;
                _roomInformationService = roomInformationService;
                _bookingReservationService = bookingReservationService;
                _bookingDetailService = bookingDetailService;
                LoadData();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadData()
        {
            lvCustomer.ItemsSource = _customerService.GetAll().ToList();
            lvRoomInformations.ItemsSource = _roomInformationService.GetAll().ToList();
            lvBookingReservations.ItemsSource = _bookingReservationService.GetAll()
                .Include(x => x.Customer)
                .Include(x => x.BookingDetails)
                .ThenInclude(x => x.Room).ToList();
        }

        private void CreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var createCustomerWindow = _serviceProvider.GetService(typeof(CreateCustomerWindow)) as CreateCustomerWindow;
            createCustomerWindow?.DelegateAction(LoadData);
            createCustomerWindow?.Show();
        }

        private void CreateRoomInformation_Click(object sender, RoutedEventArgs e)
        {
            var createRoomInformationWindow = _serviceProvider.GetService(typeof(CreateRoomInformationWindow)) as CreateRoomInformationWindow;
            createRoomInformationWindow?.DelegateAction(LoadData);
            createRoomInformationWindow?.Show();
        }

        private void CreateBookingReservation_Click(object sender, RoutedEventArgs e)
        {
            var createBookingReservationWindow = _serviceProvider.GetService(typeof(CreateBookingReservationWindow)) as CreateBookingReservationWindow;
            createBookingReservationWindow?.DelegateAction(LoadData);
            createBookingReservationWindow?.Show();
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvCustomer.SelectedItem == null)
                    return;
                var customerIdToDelete = (int)lvCustomer.SelectedValue;
                var customerToDelete = _customerService.GetAll().Include(x => x.BookingReservations).FirstOrDefault(x => x.CustomerId == customerIdToDelete) ?? throw new Exception("Customer does not exist");

                var confirmationResult = MessageBox.Show($"Are you sure to delete customer {customerToDelete.CustomerFullName}?", "Delete customer", MessageBoxButton.YesNo);
                if (confirmationResult == MessageBoxResult.No)
                    return;

                if (customerToDelete.BookingReservations.Any())
                {
                    customerToDelete.CustomerStatus = 0;
                    _customerService.Update(customerToDelete);
                }
                else
                    _customerService.Delete(customerToDelete);

                LoadData();
                MessageBox.Show("Customer deleted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteRoomInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var roomIdToDelete = (int)lvRoomInformations.SelectedValue;
                var roomToDelete = _roomInformationService.GetAll().Include(x => x.BookingDetails).FirstOrDefault(x => x.RoomId == roomIdToDelete) ?? throw new Exception("Room does not exist");

                var confirmationResult = MessageBox.Show($"Are you sure to delete room {roomToDelete.RoomNumber}?", "Delete room information", MessageBoxButton.YesNo);
                if (confirmationResult == MessageBoxResult.No)
                    return;

                if (roomToDelete.BookingDetails.Any())
                {
                    roomToDelete.RoomStatus = 2;
                    _roomInformationService.Update(roomToDelete);
                }
                else
                {
                    _roomInformationService.Delete(roomToDelete);
                }

                LoadData();
                MessageBox.Show("Room information deleted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBookingReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookingReservationIdToDelete = (int)lvBookingReservations.SelectedValue;
                var bookingReservationToDelete = _bookingReservationService.GetAll().Include(x => x.BookingDetails).FirstOrDefault(x => x.BookingReservationId == bookingReservationIdToDelete) ?? throw new Exception("Booking reservation does not exist");

                var confirmationResult = MessageBox.Show($"Are you sure to delete booking reservation with id {bookingReservationToDelete.BookingReservationId}?", "Delete booking reservation", MessageBoxButton.YesNo);
                if (confirmationResult == MessageBoxResult.No)
                    return;

                if (bookingReservationToDelete.BookingDetails.Any())
                {
                    bookingReservationToDelete.BookingStatus =2;
                    _bookingReservationService.Update(bookingReservationToDelete);
                }
                else
                    _bookingReservationService.Delete(bookingReservationToDelete);

                LoadData();
                MessageBox.Show("Booking reservation deleted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                const string phonePattern = @"^\+?\d{1,4}?[\s-.\(\)]?\(?\d{1,3}?\)?[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,9}$";
                const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (string.IsNullOrEmpty(txtCustomerId.Text))
                    return;
                var customerIdToUpdate = int.Parse(txtCustomerId.Text);
                var customerToUpdate = _customerService.GetById(x => x.CustomerId == customerIdToUpdate) ?? throw new Exception("Customer does not exist");

                customerToUpdate.CustomerFullName = txtFullName.Text;
                customerToUpdate.Telephone = txtTelephone.Text;
                customerToUpdate.EmailAddress = txtEmailAddress.Text;
                customerToUpdate.CustomerBirthday = dpBirthday.SelectedDate ?? DateTime.MinValue;

                if (string.IsNullOrEmpty(customerToUpdate.CustomerFullName))
                    throw new Exception("Customer full name is required");
                if (string.IsNullOrEmpty(customerToUpdate.EmailAddress))
                    throw new Exception("Email address is required");
                if (_customerService.GetAll().Any(x => x.EmailAddress == customerToUpdate.EmailAddress && x.CustomerId != customerToUpdate.CustomerId))
                    throw new Exception("Email address already exists");
                if (customerToUpdate.CustomerBirthday >= DateTime.Now)
                    throw new Exception("Customer birthday must be less than current date");
                if (!Regex.IsMatch(customerToUpdate.EmailAddress, emailPattern))
                {
                    throw new Exception("Invalid Email format");
                }
                if (!Regex.IsMatch(customerToUpdate.Telephone, phonePattern))
                {
                    throw new Exception("Invalid phone format");
                }

                var confirmationResult = MessageBox.Show($"Are you sure to update customer {customerToUpdate.CustomerFullName}?", "Update customer", MessageBoxButton.YesNo);
                if (confirmationResult == MessageBoxResult.No)
                    return;
                _customerService.Update(customerToUpdate);

                MessageBox.Show("Customer updated successfully");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UpdateRoomInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRoomId.Text))
                    return;

                if (!int.TryParse(txtMaxCapacity.Text, out var roomMaxCapacity))
                    throw new Exception("Room max capacity must be a number");

                var roomInformationIdToUpdate = int.Parse(txtRoomId.Text);
                var roomInformationToUpdate = _roomInformationService.GetById(x => x.RoomId == roomInformationIdToUpdate) ?? throw new Exception("Room information id does not exist");
                var oldRoomNumber = roomInformationToUpdate.RoomNumber;

                roomInformationToUpdate.RoomNumber = txtRoomNumber.Text;
                roomInformationToUpdate.RoomDetailDescription = txtRoomDetail.Text;
                roomInformationToUpdate.RoomMaxCapacity = roomMaxCapacity;

                if (string.IsNullOrEmpty(roomInformationToUpdate.RoomNumber))
                    throw new Exception("Room number is required");
                if (_roomInformationService.GetAll().Any(x => x.RoomNumber == roomInformationToUpdate.RoomNumber && x.RoomId != roomInformationToUpdate.RoomId))
                    throw new Exception("Room number already exists");

                var confirmationResult = MessageBox.Show($"Are you sure to update room {oldRoomNumber}?", "Update room information", MessageBoxButton.YesNo);
                if (confirmationResult == MessageBoxResult.No)
                    return;

                _roomInformationService.Update(roomInformationToUpdate);
                MessageBox.Show("Room information updated successfully");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateBookingReservation_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SearchCustomers_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtCustomerSearchKeyword.Text.ToLower();
            if (string.IsNullOrEmpty(keyword))
                lvCustomer.ItemsSource = _customerService.GetAll().Where(x => x.CustomerStatus != 0).ToList();
            else {
                var customers = _customerService.GetAll().Where(x => x.CustomerFullName.Contains(keyword) && x.CustomerStatus != 0);
                lvCustomer.ItemsSource = customers.ToList();
            }
        }

        private void SearchRoomInformations_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtRoomInformationSearchKeyword.Text;
            if (string.IsNullOrEmpty(keyword))
                lvRoomInformations.ItemsSource = _roomInformationService.GetAll().Where(x => x.RoomStatus != 0).ToList();
            else
                lvRoomInformations.ItemsSource = _roomInformationService.GetAll().Where(x => x.RoomStatus != 0)
                    .Include(x => x.RoomType)
                    .Where(x => x.RoomNumber.Contains(keyword) || x.RoomDetailDescription.Contains(keyword) || x.RoomType.RoomTypeName.Contains(keyword))
                    .ToList();
        }

        private void SearchBookingReservations_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtBookingReservationSearchKeyword.Text;
            if (string.IsNullOrEmpty(keyword))
                lvBookingReservations.ItemsSource = _bookingReservationService.GetAll().Where(x => x.BookingStatus != 0).ToList();
            else
                lvBookingReservations.ItemsSource = _bookingReservationService.GetAll().Where(x => x.BookingStatus != 0)
                    .Where(x => x.Customer.CustomerFullName.Contains(keyword, StringComparison.OrdinalIgnoreCase) || x.TotalPrice.ToString().Contains(keyword) || x.BookingDate.ToString().Contains(keyword))
                    .ToList();
        }
    }
}
