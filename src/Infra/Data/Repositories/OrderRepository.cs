using Domain.Entities;
using Domain.Repositories.Base;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Order>> GetOrdersByCustomerId(Guid customerId) =>
         await _dbContext.Set<Order>()
             .Where(c => c.ClientId == customerId)
             .Include(x => x.Products)
             .Include(x => x.Payment)
             .ToListAsync();
    

    public IQueryable<Order> GetAll() =>
        _dbContext.Set<Order>()
            .AsNoTracking()
            .Include(x => x.Products)
            .Include(x => x.Payment)
            .AsQueryable();


    public Order GetById(Guid id) => 
        _dbContext.Set<Order>()
            .AsNoTracking()
            .Include(x => x.Products)
            .Include(x => x.Payment)
            .FirstOrDefault(x => x.Id == id);

    public Order GetByOrderCode(int orderCode) =>
        _dbContext.Set<Order>()
            .AsNoTracking()
            .Include(x => x.Products)
            .Include(x => x.Payment)
            .FirstOrDefault(x => x.OrderCode == orderCode);


    public IQueryable<Order> Get() => _dbContext.Set<Order>()
            .AsNoTracking()
            .Include(x => x.Products)
            .Include(x => x.Payment)
            .Where(x => x.Status != OrderStatus.Finished)
            .OrderBy(x => x.Status == OrderStatus.Completed ? 1 : 2)
            .ThenBy(x => x.Status == OrderStatus.InPreparation ? 1 : 2)
            .ThenBy(x => x.Status == OrderStatus.Pending ? 1 : 2)
            .AsQueryable();

    public async Task<List<Order>> GetOrdersByCpf(string cpf) =>
        await _dbContext.Set<Order>()
            .Where(c => c.Client.CPF == cpf)
            .Include(x => x.Products)
            .Include(x => x.Payment)
            .ToListAsync();
}
