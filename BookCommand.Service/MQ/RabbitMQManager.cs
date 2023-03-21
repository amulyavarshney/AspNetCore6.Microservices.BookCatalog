using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BookCommand.Service.MQ
{
    /* An interface that is used to publish messages to RabbitMQ. */
    public interface IRabbitMQManager
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
            where T : class;
    }

    /* It's a wrapper around a RabbitMQ channel that uses an object pool to manage the channel */
    public class RabbitMQManager : IRabbitMQManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly ILogger<RabbitMQManager> _logger;

        public RabbitMQManager(IPooledObjectPolicy<IModel> objectPolicy, ILoggerFactory loggerFactory)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount);
            _logger = loggerFactory.CreateLogger<RabbitMQManager>();
        }

        /// <summary>
        /// It takes a message, an exchange name, an exchange type, and a route key, and publishes the
        /// message to the exchange
        /// </summary>
        /// <param name="T">The type of the message to be published.</param>
        /// <param name="exchangeName">The name of the exchange.</param>
        /// <param name="exchangeType"></param>
        /// <param name="routeKey">The routing key is a message attribute. The routing key is used for
        /// routing messages depending on the exchange configuration.</param>
        /// <returns>
        /// The channel is being returned to the object pool.
        /// </returns>
        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                /* It's declaring an exchange. */
                channel.ExchangeDeclare(exchangeName, exchangeType, false, false, null);

                /* It's converting the message to a byte array. */
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                /* It's setting the message to be persistent. */
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                /* It's publishing a message to the exchange. */
                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception e)
            {
                /* It's logging the exception message. */
                _logger.LogError($"Exception: {e.Message}");
            }
            finally
            {
                /* It's returning the channel to the object pool. */
                _objectPool.Return(channel);
            }
        }
    }
}
