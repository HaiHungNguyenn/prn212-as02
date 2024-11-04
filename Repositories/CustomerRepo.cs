using System.Linq.Expressions;
using BusinessObjects;
using DAL;
using Repositories.IRepository;

namespace Repositories;

public class CustomerRepo : ICustomerRepo
{
    public Customer? GetById(Expression<Func<Customer, bool>> predicate) => CustomerDao.Instance.GetById(predicate);
    public IQueryable<Customer> GetAll() => CustomerDao.Instance.GetAll();
    public void Create(Customer entity) => CustomerDao.Instance.Create(entity);
    public void Update(Customer entity) => CustomerDao.Instance.Update(entity);
    public void Delete(Customer entity) => CustomerDao.Instance.Delete(entity);
}