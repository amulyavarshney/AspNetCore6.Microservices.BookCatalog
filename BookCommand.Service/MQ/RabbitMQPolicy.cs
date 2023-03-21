using BookCommand.Service.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BookCommand.Service.MQ
{
    /* The RabbitMQPolicy class implements the IPooledObjectPolicy interface. The IPooledObjectPolicy
    interface has two methods: Create and Return. The Create method is called when the pool needs to
    create a new object. The Return method is called when the pool needs to determine if an object
    is still valid */
    public class RabbitMQPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMQConfig _rabbitMQConfig;

        private readonly IConnection _connection;

        public RabbitMQPolicy(IOptions<RabbitMQConfig> options)
        {
            _rabbitMQConfig = options.Value;
            _connection = GetConnection();
        }

        /// <summary>
        /// It creates a connection to the RabbitMQ server
        /// </summary>
        /// <returns>
        /// A connection to the RabbitMQ server.
        /// </returns>
        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                Port = _rabbitMQConfig.Port,
                VirtualHost = _rabbitMQConfig.VirtualHost,
            };

            return factory.CreateConnection();
        }

        /// <summary>
        /// It creates a new channel to the RabbitMQ server
        /// </summary>
        /// <returns>
        /// A new instance of the IModel interface.
        /// </returns>
        public IModel Create()
        {
            return _connection.CreateModel();
        }

        /// <summary>
        /// If the object is open, return true. If the object is not open, dispose of the object and
        /// return false
        /// </summary>
        /// <param name="IModel">The object to be returned to the pool.</param>
        /// <returns>
        /// A boolean value.
        /// </returns>
        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
