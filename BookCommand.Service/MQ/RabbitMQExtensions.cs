using BookCommand.Service.Models;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace BookCommand.Service.MQ
{
    /* It adds the RabbitMQConfig to the service collection, and then adds the RabbitMQManager to the
    service collection */
    public static class RabbitMQExtensions
    {
        /// <summary>
        /// It adds a singleton instance of the RabbitMQManager class to the service collection
        /// </summary>
        /// <param name="IServiceCollection">This is the collection of services that are registered with
        /// the dependency injection container.</param>
        /// <param name="IConfiguration">This is the configuration object that you can use to read the
        /// configuration from the appsettings.json file.</param>
        /// <returns>
        /// The IServiceCollection object.
        /// </returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQConfig");
            
            /* Adding the RabbitMQConfig to the service collection. */
            services.Configure<RabbitMQConfig>(rabbitMQConfig);

            /* Adding a singleton instance of the DefaultObjectPoolProvider class to the service
            collection. */
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            
            /* Adding a singleton instance of the RabbitMQPolicy class to the service collection. */
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMQPolicy>();

            /* Adding a singleton instance of the RabbitMQManager class to the service collection. */
            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

            return services;
        }
    }
}
