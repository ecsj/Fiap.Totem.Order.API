using Domain.Entities;
using Domain.Request;
using Domain.Response;

namespace Application.Interfaces;

public interface IOrderUseCase
{
    Task<OrderResponse> PlaceOrder(OrderRequest request);
    Task<List<Order>> GetOrdersByCustomer(Guid customerId);
    Task<List<Order>> GetOrdersByCpf(string cpf);
    IQueryable<Order> Get();
    Task<OrderResponse> GetById(Guid orderId);
    IList<Order> GetOrdersPending();
    IList<Order> GetOrdersByStatus(OrderStatus status);
    Task UpdateOrderStatus(Guid orderId, OrderStatus newStatus);
    Task UpdatePayment(Guid orderId, PaymentResponse paymentResponse);
    Task CancelOrder(Guid orderId);
    Task<OrderResponse> GetByOrderCode(int orderCode);
}