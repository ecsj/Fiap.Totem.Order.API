using Application.Interfaces;
using Domain.Entities;
using Domain.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Domain.Request;

namespace Application.BackgroundServices;

[ExcludeFromCodeCoverage]
public class CustomerDeletedHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaidOrderHandler> _logger;
    private readonly IMessageQueueService _messageQueueService;


    public CustomerDeletedHandler(IServiceProvider serviceProvider, ILogger<PaidOrderHandler> logger, IMessageQueueService messageQueueService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _messageQueueService = messageQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Waiting for Messages");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            async Task HandleMessage(string message)
            {
                var clientRequest = JsonSerializer.Deserialize<ClientRequest>(message);

                using var scope = _serviceProvider.CreateScope();

                var clientUseCase = scope.ServiceProvider.GetRequiredService<IClientUseCase>();

                await clientUseCase.Delete(clientRequest.Id);

                Console.WriteLine("Mensagem recebida: " + clientRequest);
            }

            _messageQueueService.ConsumeMessages("Totem.Customer.Deleted", HandleMessage);

            await Task.Delay(1000, stoppingToken);

        }
    }
}