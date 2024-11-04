using Repositories;
using Repositories.IRepository;
using Services;
using Services.IService;

namespace WebApplication1.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerRepo, CustomerRepo>();
        services.AddScoped<IRoomInformationService, RoomInformationService>();
        services.AddScoped<IRoomInformationRepo, RoomInformationRepo>();
        services.AddScoped<IRoomTypeService, RoomTypeService>();
        services.AddScoped<IRoomTypeRepo, RoomTypeRepo>();
        services.AddScoped<IBookingReservationService, BookingReservationService>();
        services.AddScoped<IBookReservationRepo, BookReservationRepo>();
        services.AddScoped<IBookingDetailService, BookingDetailService>();
        services.AddScoped<IBookingDetailRepo, BookDetailRepo>();
        return services;
    }
}