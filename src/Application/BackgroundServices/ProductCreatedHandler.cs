using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Interfaces;
using Domain.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.BackgroundServices;

[ExcludeFromCodeCoverage]
public class ProductCreatedHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProductCreatedHandler> _logger;
    private readonly IMessageQueueService _messageQueueService;

    public ProductCreatedHandler(IServiceProvider serviceProvider, ILogger<ProductCreatedHandler> logger, IMessageQueueService messageQueueService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _messageQueueService = messageQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Waiting for Orders Pending");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            await _messageQueueService.ConsumeMessages("Totem.Products.Created", async (message) =>
            {
                var product = JsonSerializer.Deserialize<ProductRequest>(message);

                using var scope = _serviceProvider.CreateScope();

                var productUseCase = scope.ServiceProvider.GetRequiredService<IProductUseCase>();

                await productUseCase.Add(product);

                _logger.LogInformation($"Message received: {product}");
            });

            await Task.Delay(1000, stoppingToken);

        }
    }
}