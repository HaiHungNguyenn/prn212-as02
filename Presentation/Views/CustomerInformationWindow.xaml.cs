using System.Text.RegularExpressions;
using System.Windows;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for CustomerInformationWindow.xaml
    /// </summary>
    public partial class CustomerInformationWindow : Window
    {
		private readonly ICustomerService _customerService;
		private readonly IBookingReservationService _bookingReservationService;

		public  Customer Customer { get; set; }

        public CustomerInformationWindow(ICustomerService customerService, IBookingReservationService bookingReservationService)
		{
			InitializeComponent();
			_customerService = customerService;
			_bookingReservationService = bookingReservationService;
		}

		public void LoadData()
		{
			txtFullName.Text = Customer.CustomerFullName;
			txtEmailAddress.Text = Customer.EmailAddress;
			txtTelephone.Text = Customer.Telephone;
			dpBirthDate.SelectedDate = Customer.CustomerBirthday;
			dpBirthDate.DisplayDate = Customer.CustomerBirthday ?? DateTime.Now;
			txtPassword.Text = Customer.Password;

			lvBookingReservations.ItemsSource = _bookingReservationService.GetAll()
				.Include(x => x.Customer)
				.Where(x => x.CustomerId == Customer.CustomerId).ToList();
		}

		private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
		{
			try 
			{
                string phonePattern = @"^\+?\d{1,4}?[\s-.\(\)]?\(?\d{1,3}?\)?[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,9}$";
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if (string.IsNullOrEmpty(txtFullName.Text))
					throw new Exception("Full name is required");
				if (string.IsNullOrEmpty(txtEmailAddress.Text))
					throw new Exception("Email address is required");
				if (Regex.IsMatch(txtEmailAddress.Text, emailPattern, RegexOptions.IgnoreCase)) {
					throw new Exception("Invalid email format"); 
				}
				if (string.IsNullOrEmpty(txtTelephone.Text)) {
					throw new Exception("Phone number is required");
				}
				if (!Regex.IsMatch(txtTelephone.Text, phonePattern)) { 
					throw new Exception("Phone must have 10 digits only");
				}
				if (dpBirthDate.SelectedDate >= DateTime.Now)
					throw new Exception("Birthday must be before today");
				if (string.IsNullOrEmpty(txtPassword.Text))
					throw new Exception("Password is required");

				if (_customerService.GetAll().Any(x => x.EmailAddress == txtEmailAddress.Text && x.CustomerId != Customer.CustomerId))
					throw new Exception("Email address already exists");

				var confirmationResult = MessageBox.Show($"Are you sure to update your information?", "Update information", MessageBoxButton.YesNo);
				if (confirmationResult == MessageBoxResult.No)
					return;
				Customer.CustomerFullName = txtFullName.Text;
				Customer.EmailAddress = txtEmailAddress.Text;
				Customer.Telephone = txtTelephone.Text;
				Customer.CustomerBirthday = dpBirthDate.SelectedDate;
				Customer.Password = txtPassword.Text;
				_customerService.Update(Customer);

				MessageBox.Show("Information updated successfully");
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ResetInformation_Click(object sender, RoutedEventArgs e)
		{
			LoadData();
		}
    }
}
