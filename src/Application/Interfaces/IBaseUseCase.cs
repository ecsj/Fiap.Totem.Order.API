using Domain.Entities;

namespace Application.Interfaces;

public interface IBaseUseCase<TIn, TOut>
{
    IQueryable<TOut> Get();
    Task<TOut> GetById(Guid id);
    Task<TOut> Add(TIn request);
    Task Update(Guid id, TIn request);
    Task Delete(Guid id);
}
