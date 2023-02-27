using BookCommand.Service.Models;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace BookCommand.Service.MQ
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQConfig");
            services.Configure<RabbitMQConfig>(rabbitMQConfig);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMQPolicy>();

            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

            return services;
        }
    }
}
