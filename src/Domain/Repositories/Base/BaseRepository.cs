namespace Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    IQueryable<TEntity> Get();
    Task<TEntity> GetByIdAsync<TId>(TId id) where TId : notnull;
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<int> SaveChangesAsync();
}