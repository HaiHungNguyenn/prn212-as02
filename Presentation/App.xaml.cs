using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Views;
using Repositories;
using Repositories.IRepository;
using Services;
using Services.IService;

namespace Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider serviceProvider;

    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // services.AddSingleton(typeof(IRepo<>),typeof(Repo<>));
        services.AddSingleton<LoginWindow>();
        services.AddTransient<CreateCustomerWindow>();
        services.AddTransient<CreateRoomInformationWindow>();
        services.AddTransient<CreateBookingReservationWindow>();
        services.AddTransient<CustomerInformationWindow>();
        services.AddSingleton<MainManagementWindow>();
        // services.AddSingleton<FuminiHotelManagementContext>();
        services.AddScoped<IBookingDetailRepo,BookDetailRepo>();
        services.AddScoped<IBookReservationRepo,BookReservationRepo>();
        services.AddScoped<ICustomerRepo,CustomerRepo>();
        services.AddScoped<IRoomInformationRepo,RoomInformationRepo>();
        services.AddScoped<IRoomTypeRepo, RoomTypeRepo>();

        services.AddScoped<IBookingDetailService, BookingDetailService>();
        services.AddScoped<IBookingReservationService, BookingReservationService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IRoomInformationService, RoomInformationService>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
    }

    private void OnStartUp(object sender, StartupEventArgs e)
    {
        var LoginWindow = serviceProvider.GetService<LoginWindow>();
        LoginWindow?.Show();
    }
}