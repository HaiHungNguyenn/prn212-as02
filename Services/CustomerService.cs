using System.Linq.Expressions;
using BusinessObjects;
using Repositories.IRepository;
using Services.IService;

namespace Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepo _customerRepo;

    public CustomerService(ICustomerRepo customerRepo)
    {
        _customerRepo = customerRepo;
    }
    public Customer? GetById(Expression<Func<Customer, bool>> predicate) => _customerRepo.GetById(predicate);
    public IQueryable<Customer> GetAll() => _customerRepo.GetAll();

    public void Create(Customer entity) => _customerRepo.Create(entity);

    public void Update(Customer entity) => _customerRepo.Update(entity);

    public void Delete(Customer entity) => _customerRepo.Delete(entity);
}