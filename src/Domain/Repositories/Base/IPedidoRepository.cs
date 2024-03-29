using Domain.Entities;

namespace Domain.Repositories.Base;

public interface IOrderRepository : IRepository<Order>
{
    IQueryable<Order> GetAll();
    Order GetById(Guid id);
    Order GetByOrderCode(int orderCode);
    Task<List<Order>> GetOrdersByCustomerId(Guid customerId);
    Task<List<Order>> GetOrdersByCpf(string cpf);
}