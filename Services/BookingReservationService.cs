using System.Linq.Expressions;
using Repositories.IRepository;
using Services.IService;

namespace Services;

public class BookingReservationService : IBookingReservationService
{
    private readonly IBookReservationRepo _bookReservationRepo;

    public BookingReservationService(IBookReservationRepo bookReservationRepo)
    {
        _bookReservationRepo = bookReservationRepo;
    }

    public BookingReservation? GetById(Expression<Func<BookingReservation, bool>> predicate) =>
        _bookReservationRepo.GetById(predicate);
    public IQueryable<BookingReservation> GetAll() => _bookReservationRepo.GetAll();

    public void Create(BookingReservation entity) => _bookReservationRepo.Create(entity);

    public void Update(BookingReservation entity) => _bookReservationRepo.Update(entity);

    public void Delete(BookingReservation entity) => _bookReservationRepo.Delete(entity);
}