using System.Linq.Expressions;

namespace Repositories.IRepository;

public interface IRepo <T> where T: class
{
    T? GetById(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}