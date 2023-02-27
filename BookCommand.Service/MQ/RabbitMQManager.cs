using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BookCommand.Service.MQ
{
    public interface IRabbitMQManager
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
            where T : class;
    }

    public class RabbitMQManager : IRabbitMQManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly ILogger<RabbitMQManager> _logger;

        public RabbitMQManager(IPooledObjectPolicy<IModel> objectPolicy, ILoggerFactory loggerFactory)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount);
            _logger = loggerFactory.CreateLogger<RabbitMQManager>();
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, false, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
