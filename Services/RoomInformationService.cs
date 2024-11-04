using System.Linq.Expressions;
using Repositories.IRepository;
using Services.IService;

namespace Services;

public class RoomInformationService : IRoomInformationService
{
    private readonly IRoomInformationRepo _roomInformationRepo;

    public RoomInformationService(IRoomInformationRepo roomInformationRepo)
    {
        _roomInformationRepo = roomInformationRepo;
    }

    public RoomInformation? GetById(Expression<Func<RoomInformation, bool>> predicate)
        => _roomInformationRepo.GetById(predicate);
    public IQueryable<RoomInformation> GetAll() => _roomInformationRepo.GetAll();
    public void Create(RoomInformation entity) => _roomInformationRepo.Create(entity);
    public void Update(RoomInformation entity) => _roomInformationRepo.Update(entity);
    public void Delete(RoomInformation entity) => _roomInformationRepo.Delete(entity);
}