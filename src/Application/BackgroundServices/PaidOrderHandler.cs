using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.BackgroundServices;

[ExcludeFromCodeCoverage]
public class PaidOrderHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaidOrderHandler> _logger;

    public PaidOrderHandler(IServiceProvider serviceProvider, ILogger<PaidOrderHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Waiting for Orders Pending");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var orderUseCase = scope.ServiceProvider.GetRequiredService<IOrderUseCase>();

            var orders = orderUseCase.GetOrdersByStatus(OrderStatus.AuthorizedPayment);

            foreach (var order in orders)
            {
                _logger.LogInformation($"Processing order Id={order.Id} ");

                await orderUseCase.UpdateOrderStatus(order.Id, Domain.Entities.OrderStatus.InPreparation);
            }
            await Task.Delay(1000, stoppingToken);

        }
    }
}