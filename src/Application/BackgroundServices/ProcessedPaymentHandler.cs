using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Interfaces;
using Domain.Request;
using Domain.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.BackgroundServices;

[ExcludeFromCodeCoverage]
public class ProcessedPaymentHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProcessedPaymentHandler> _logger;
    private readonly IMessageQueueService _messageQueueService;

    public ProcessedPaymentHandler(IServiceProvider serviceProvider, ILogger<ProcessedPaymentHandler> logger, IMessageQueueService messageQueueService)
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

            async Task HandleMessage(string message)
            {
                var payment = JsonSerializer.Deserialize<PaymentResponse>(message);

                using var scope = _serviceProvider.CreateScope();

                var productUseCase = scope.ServiceProvider.GetRequiredService<IOrderUseCase>();

                await Task.Delay(5000);

                await productUseCase.UpdatePayment(payment.OrderId, payment);

                Console.WriteLine("Mensagem recebida: " + payment);
            }

            _messageQueueService.ConsumeMessages("Totem.Payment.ProcessedPayment", HandleMessage);
            
            await Task.Delay(1000, stoppingToken);

        }
    }
}