using Domain.Repositories.Base;
using Infra.Data;

namespace Infra.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        return entity;
    }

    public IQueryable<T> Get()
    {
        var entitySet = _dbContext.Set<T>();

        return entitySet;
    }

    public IQueryable<T> Get(Func<T, bool> predicate)
    {
        return Get().Where(predicate).AsQueryable();
    }

    public virtual async Task<T> GetByIdAsync<TId>(TId id) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id });
    }
    public virtual async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);

        await SaveChangesAsync();
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
