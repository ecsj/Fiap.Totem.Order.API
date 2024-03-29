using Application.Interfaces;
using Application.Interfaces.ApiClients;
using Domain.Base;
using Domain.Entities;
using Domain.Repositories.Base;
using Domain.Request;
using Domain.Response;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

public class OrderUseCase : IOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IClientUseCase _clientUseCase;
    private readonly ILogger<OrderUseCase> _logger;
    private readonly IPaymentApi _paymentApi;
    private readonly IMessageQueueService _messageQueueService;

    public OrderUseCase(IOrderRepository orderRepository,
                        IRepository<Product> productRepository,
                        IClientUseCase clientUseCase,
                        ILogger<OrderUseCase> logger, IPaymentApi paymentApi, IMessageQueueService messageQueueService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _clientUseCase = clientUseCase;
        _logger = logger;
        _paymentApi = paymentApi;
        _messageQueueService = messageQueueService;
    }

    public async Task<OrderResponse> PlaceOrder(OrderRequest request)
    {
        try
        {
            var order = Order.FromOrderRequest(request);

            foreach (var item in order.Products)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product is null)
                    throw new DomainException("Produto não encontrado");

            }

            if (order.ClientId is not null)
            {
                var client = await _clientUseCase.GetById((Guid)order.ClientId);

                if(client is null) 
                    throw new DomainException("Cliente não encontrado");

            }

            await _orderRepository.AddAsync(order);

            var paymentResponse = await _paymentApi.ProcessPayment(new PaymentRequest(order));
                
            order.UpdatePayment(paymentResponse.Status, paymentResponse.QrCode);
            
            await _orderRepository.SaveChangesAsync();

            return new OrderResponse().GenerateResponse(order);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar Pedido", ex);
            throw;
        }
    }

    public IQueryable<Order> Get()
    {
        return _orderRepository.Get();
    }
    public IList<Order> GetOrdersByStatus(OrderStatus status)
    {
        return _orderRepository.GetAll().Where(o => o.Status == status).ToList();
    }
    public IList<Order> GetOrdersPending()
    {
        return _orderRepository.GetAll().Where(o => o.Status == OrderStatus.Pending).ToList();
    }

    public async Task<List<Order>> GetOrdersByCustomer(Guid customerId)
    {
        return await _orderRepository.GetOrdersByCustomerId(customerId);
    }

    public async Task<List<Order>> GetOrdersByCpf(string cpf)
    {
        return await _orderRepository.GetOrdersByCpf(cpf);
    }
    public async Task<OrderResponse> GetById(Guid orderId)
    {
        var order = _orderRepository.GetById(orderId);

        return new OrderResponse().GenerateResponse(order);
    }

    public async Task<OrderResponse> GetByOrderCode(int orderId)
    {
        var order = _orderRepository.GetByOrderCode(orderId);

        return new OrderResponse().GenerateResponse(order);
    }


    public async Task UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        order.ChangeStatus(newStatus);

        await _orderRepository.UpdateAsync(order);
    }

    public async Task UpdatePayment(Guid orderId, PaymentResponse paymentResponse)
    {
        var order = _orderRepository.GetById(orderId);

        if(order is null) return;

        order.UpdatePayment(paymentResponse.Status, paymentResponse.QrCode);

        if (paymentResponse.Status == PaymentStatus.Approved)
        {
            order.ChangeStatus(OrderStatus.AuthorizedPayment);

            _messageQueueService.PublishMessage("Totem.Order.AuthorizedPayment", order.ToJson());
        }
        else
        {
            order.ChangeStatus(OrderStatus.UnauthorizedPayment);

            _messageQueueService.PublishMessage("Totem.Order.UnauthorizedPayment", order.ToJson());
        }

        await _orderRepository.UpdateAsync(order);

        await _orderRepository.SaveChangesAsync();
    }

    public async Task CancelOrder(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        order.ChangeStatus(OrderStatus.Canceled);

        await _orderRepository.UpdateAsync(order);
    }
}