using System.Linq.Expressions;
using BusinessObjects;
using Repositories.IRepository;
using Services.IService;

namespace Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepo _roomTypeRepo;

    public RoomTypeService(IRoomTypeRepo roomTypeRepo)
    {
        _roomTypeRepo = roomTypeRepo;
    }
    public RoomType? GetById(Expression<Func<RoomType, bool>> predicate) => _roomTypeRepo.GetById(predicate);
    public IQueryable<RoomType> GetAll() => _roomTypeRepo.GetAll();
    public void Create(RoomType entity) => _roomTypeRepo.Create(entity);
    public void Update(RoomType entity) => _roomTypeRepo.Update(entity);
    public void Delete(RoomType entity) => _roomTypeRepo.Delete(entity);
}