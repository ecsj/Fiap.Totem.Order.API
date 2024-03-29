using Application.BackgroundServices;
using Application.Interfaces;
using Application.Interfaces.ApiClients;
using Application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace Application;

[ExcludeFromCodeCoverage]
public class Dependencies
{
    public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        
        services.AddScoped<IClientUseCase, ClientUseCase>();
        services.AddScoped<IOrderUseCase, OrderUseCase>();
        services.AddScoped<IProductUseCase, ProductUseCase>();
        
        services.AddHostedService<ProductCreatedHandler>();
        services.AddHostedService<ProcessedPaymentHandler>();
        services.AddHostedService<PaidOrderHandler>();


        services.AddRefitClient<IPaymentApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["PaymentApiUrl"]));

        return services;
    }
}

