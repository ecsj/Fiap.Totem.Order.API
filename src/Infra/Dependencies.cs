using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories.Base;
using Infra.Repositories;
using Application.Interfaces;
using Infra.MessageQueue;
using System.Diagnostics.CodeAnalysis;

namespace Infra
{

    [ExcludeFromCodeCoverage]
    public class Dependencies
    {
        public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            bool useOnlyInMemoryDatabase = configuration.GetValue<bool?>("UseOnlyInMemoryDatabase") ?? false;

            if (useOnlyInMemoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(c =>
                    c.UseInMemoryDatabase("TechChallengeTotem"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            }

            services.AddSingleton<IMessageQueueService, RabbitMQService>();


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            return services;
        }
    }
}
