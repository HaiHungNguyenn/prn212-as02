using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class BaseDao<T> where T : class
{
    private readonly FuminiHotelManagementContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public BaseDao(FuminiHotelManagementContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    
    public T? GetById(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public IQueryable<T> GetAll()
    {
        try
        {
            return _dbSet.AsQueryable();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Create(T entity)
    {
        try
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Update(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public void Delete(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}