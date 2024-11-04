using System.Linq.Expressions;
using BusinessObjects;
using DAL;
using Repositories.IRepository;

namespace Repositories;

public class BookDetailRepo : IBookingDetailRepo
{
    public BookingDetail? GetById(Expression<Func<BookingDetail, bool>> predicate) => BookingDetailDao.Instance.GetById(predicate);
    public IQueryable<BookingDetail> GetAll() => BookingDetailDao.Instance.GetAll();
    public void Create(BookingDetail entity) => BookingDetailDao.Instance.Create(entity);
    public void Update(BookingDetail entity) => BookingDetailDao.Instance.Update(entity);
    public void Delete(BookingDetail entity) => BookingDetailDao.Instance.Delete(entity);
}