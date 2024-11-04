using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Repositories.IRepository;
using BusinessObjects;
using Services.IService;

namespace Presentation.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IServiceProvider _provider;
        private readonly ICustomerService _customerService;

        public LoginWindow(IServiceProvider provider, ICustomerService customerService)
        {
            _provider = provider;
            _customerService = customerService;
            InitializeComponent();
        }
       

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            IConfiguration conf = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var adminEmail = conf["admin_account:email"];
            var adminPassword = conf["admin_account:password"];
            var email = txtEmail.Text;
            var pass = txtPassword.Password.ToString();
            bool isAdminAccount = email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && pass.Equals(adminPassword);
            var customer = _customerService.GetAll().Where(x => x.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
            if (isAdminAccount)
            {
				var mainManageWindow = _provider.GetService<MainManagementWindow>();
				mainManageWindow!.Show();
				this.Close();
            }
            else if (customer != null && customer.Password == pass)
            {
				var customerInformationWindow = _provider.GetService<CustomerInformationWindow>();
				customerInformationWindow!.Customer = customer!;
				customerInformationWindow!.LoadData();
				customerInformationWindow!.Show();
				this.Close();
            }
			else
			{
				lbNoti.Content = "Invalid email or password";
				txtEmail.Text = string.Empty;
				txtEmail.Focus();
			}

        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
