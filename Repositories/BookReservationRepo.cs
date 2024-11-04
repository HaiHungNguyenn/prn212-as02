using System.Linq.Expressions;
using DAL;
using Repositories.IRepository;

namespace Repositories;

public class BookReservationRepo : IBookReservationRepo
{
    public BookingReservation? GetById(Expression<Func<BookingReservation, bool>> predicate) =>
        BookingReservationDao.Instance.GetById(predicate);
    public IQueryable<BookingReservation> GetAll() => BookingReservationDao.Instance.GetAll();
    public void Create(BookingReservation entity) => BookingReservationDao.Instance.Create(entity);
    public void Update(BookingReservation entity) => BookingReservationDao.Instance.Update(entity);
    public void Delete(BookingReservation entity) => BookingReservationDao.Instance.Delete(entity);
}