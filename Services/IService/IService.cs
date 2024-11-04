using System.Linq.Expressions;

namespace Services.IService;

public interface IService<T> where T: class
{
    T? GetById(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}