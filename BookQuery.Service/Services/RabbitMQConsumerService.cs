using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using BookQuery.Service.Models;
using Newtonsoft.Json;
using BookQuery.Service.ViewModels;

namespace BookQuery.Service.Services
{
    public class RabbitMQConsumerService : BackgroundService, IDisposable
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private readonly RabbitMQConfig _rabbitMQConfig;
        private IConnection _connection;
        private IModel _channel;

        private readonly IBookUpdateService _service;

        public RabbitMQConsumerService(ILoggerFactory loggerFactory, IOptions<RabbitMQConfig> options, IBookUpdateService service)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQConsumerService>();
            _rabbitMQConfig = options.Value;
            _service = service;
            InitializeMessageQueue();
        }

        private void InitializeMessageQueue()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
                VirtualHost = _rabbitMQConfig.VirtualHost,
                Port = _rabbitMQConfig.Port
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("ms-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("ms-queue", false, false, false, null);
            _channel.QueueBind("ms-queue", "ms-exchange", "cqrs", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogInformation($"Message queue connection shutting down...");
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, args) =>
            {
                var messageString = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<MessageViewModel>(messageString);

                _logger.LogInformation($"Message received: {Environment.NewLine}{message.BookId}{Environment.NewLine}{message.Title}{Environment.NewLine}{message.Command}");

                Console.WriteLine($"{message.BookId}, {message.Title}, {message.Description}, {message.Author}");
                await _service.UpdateAsync(message);

                _channel.BasicAck(args.DeliveryTag, false);
            };

            //  Also consider other events on consumer

            _channel.BasicConsume("ms-queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
