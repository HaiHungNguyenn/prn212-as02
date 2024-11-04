using Repositories.IRepository;
using System.Windows;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for CreateRoomInformationWindow.xaml
    /// </summary>
    public partial class CreateRoomInformationWindow : Window
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IRoomInformationService _roomInformationService;

        private Action _delegate;

        public void DelegateAction(Action action)
        {
            _delegate += action;
        }

        public CreateRoomInformationWindow(IRoomTypeService roomTypeService, IRoomInformationService roomInformationService)
        {
            InitializeComponent();
            _roomTypeService = roomTypeService;
            _roomInformationService = roomInformationService;
            PopulateData();
        }

        private void CreateRoomInformatioin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var isValidRoomCapacity = int.TryParse(txtRoomCapacity.Text, out var roomCapacity);
                if (!isValidRoomCapacity || roomCapacity <= 0)
                    throw new Exception("Room capacity must be greater than 0");

                var isValidRoomPrice = int.TryParse(txtRoomPricePerDay.Text, out var roomPrice);
                if (!isValidRoomPrice || roomPrice < 0)
                    throw new Exception("Room price must be greater than or equal to 0");

                if (string.IsNullOrEmpty(txtRoomNumber.Text)) {
                    throw new Exception("Room number is required");
                }

                var existRoom = _roomInformationService.GetAll().FirstOrDefault(x => x.RoomNumber == txtRoomNumber.Text);
                if(existRoom != null)
                {
                    throw new Exception("Room number is already exist. Please input another room number.");
                }

                var roomInformation = new RoomInformation
                {
                    RoomNumber = txtRoomNumber.Text,
                    RoomDetailDescription = txtRoomDescription.Text,
                    RoomMaxCapacity = roomCapacity,
                    RoomTypeId = (int)cbRoomTypes.SelectedValue,
                    RoomStatus = 1,
                    RoomPricePerDay = roomPrice
                };
                _roomInformationService.Create(roomInformation);

                _delegate.Invoke();
                MessageBox.Show("Room information created successfully");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PopulateData()
        {
            cbRoomTypes.ItemsSource = _roomTypeService.GetAll().ToList();
            cbRoomTypes.SelectedIndex = 0;
        }
    }
}
