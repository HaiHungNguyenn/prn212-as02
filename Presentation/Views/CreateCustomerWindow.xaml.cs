using BusinessObjects;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for CreateCustomerWindow.xaml
    /// </summary>
    public partial class CreateCustomerWindow : Window
    {
        private readonly ICustomerService _service;
        private Action _delegate;

        public void DelegateAction(Action action)
        {
            _delegate += action;
        }

        public CreateCustomerWindow(ICustomerService service)
        {
            InitializeComponent();
            _service = service;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                const string phonePattern = @"^\+?\d{1,4}?[\s-.\(\)]?\(?\d{1,3}?\)?[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,4}[\s-.\(\)]?\d{1,9}$";
                const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                var customer = new Customer
                {
                    CustomerFullName = txtFullName.Text,
                    Telephone = txtPhone.Text,
                    EmailAddress = txtEmail.Text,
                    Password = txtPassword.Text,
                    CustomerBirthday = dpBirthdate.SelectedDate,
                    CustomerStatus = 1
                };

                if (string.IsNullOrEmpty(customer.CustomerFullName))
                    throw new Exception("Customer name can not be empty");

                if (string.IsNullOrEmpty(customer.EmailAddress))
                    throw new Exception("Email can not be empty");

                if (string.IsNullOrEmpty(customer.Telephone))
                    throw new Exception("Phone number cannot be empty");

                if (string.IsNullOrEmpty(customer.Password))
                    throw new Exception("Password can not be empty");

                if (!Regex.IsMatch(customer.Telephone, phonePattern))
                    throw new Exception("Phone number must have 10 digits only");

                if (!Regex.IsMatch(customer.EmailAddress, emailPattern, RegexOptions.IgnoreCase))
                    throw new Exception("Invalid email format");

                if (customer.CustomerBirthday >= DateTime.Now)
                    throw new Exception("Birth date must before today");

                var emailExists = _service.GetAll()
                    .Any(x => x.EmailAddress == customer.EmailAddress);

                if (emailExists)
                    throw new Exception("Customer email already exists");

                _service.Create(customer);
                MessageBox.Show("Customer created successfully");
                _delegate.Invoke();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
