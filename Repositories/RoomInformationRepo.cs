using System.Linq.Expressions;
using DAL;
using Repositories.IRepository;

namespace Repositories;

public class RoomInformationRepo : IRoomInformationRepo
{
    public RoomInformation? GetById(Expression<Func<RoomInformation, bool>> predicate) => RoomInformationDao.Instance.GetById(predicate);
    public IQueryable<RoomInformation> GetAll() => RoomInformationDao.Instance.GetAll();
    public void Create(RoomInformation entity) => RoomInformationDao.Instance.Create(entity);
    public void Update(RoomInformation entity) => RoomInformationDao.Instance.Update(entity);
    public void Delete(RoomInformation entity) => RoomInformationDao.Instance.Delete(entity);
}