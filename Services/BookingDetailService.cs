using System.Linq.Expressions;
using BusinessObjects;
using Repositories.IRepository;
using Services.IService;

namespace Services;

public class BookingDetailService : IBookingDetailService
{
    private readonly IBookingDetailRepo _bookingDetailRepo;

    public BookingDetailService(IBookingDetailRepo bookingDetailRepo)
    {
        _bookingDetailRepo = bookingDetailRepo;
    }
    
    public BookingDetail? GetById(Expression<Func<BookingDetail, bool>> predicate) => _bookingDetailRepo.GetById(predicate);

    public IQueryable<BookingDetail> GetAll() => _bookingDetailRepo.GetAll();

    public void Create(BookingDetail entity) => _bookingDetailRepo.Create(entity);

    public void Update(BookingDetail entity) => _bookingDetailRepo.Update(entity);
    
    public void Delete(BookingDetail entity) => _bookingDetailRepo.Delete(entity);
}