using System.Linq.Expressions;
using BusinessObjects;
using DAL;
using Repositories.IRepository;

namespace Repositories;

public class RoomTypeRepo : IRoomTypeRepo
{
    public RoomType? GetById(Expression<Func<RoomType, bool>> predicate) => RoomTypeDao.Instance.GetById(predicate);
    public IQueryable<RoomType> GetAll() => RoomTypeDao.Instance.GetAll();
    public void Create(RoomType entity) => RoomTypeDao.Instance.Create(entity);
    public void Update(RoomType entity) => RoomTypeDao.Instance.Update(entity);
    public void Delete(RoomType entity) => RoomTypeDao.Instance.Delete(entity);
}