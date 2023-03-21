using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using BookQuery.Service.Models;
using Newtonsoft.Json;
using BookQuery.Service.ViewModels;

namespace BookQuery.Service.Services
{
    /* It's a background service that listens to a RabbitMQ queue and when a message is received, it
    deserializes the message and calls the UpdateAsync method of the IBookUpdateService interface */
    public class RabbitMQConsumerService : BackgroundService, IDisposable
    {
        /* Creating a connection to the RabbitMQ server and creating a channel. */
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private readonly RabbitMQConfig _rabbitMQConfig;
        private IConnection _connection;
        private IModel _channel;

        private readonly IBookUpdateService _service;

        public RabbitMQConsumerService(ILoggerFactory loggerFactory, IOptions<RabbitMQConfig> options, IBookUpdateService service)
        {
            /* It's creating a logger, getting the RabbitMQ configuration from the appsettings.json
            file, and initializing the message queue. */
            _logger = loggerFactory.CreateLogger<RabbitMQConsumerService>();
            _rabbitMQConfig = options.Value;
            _service = service;
            InitializeMessageQueue();
        }

        /// <summary>
        /// It creates a connection to the RabbitMQ server, creates a channel, declares an exchange,
        /// declares a queue, binds the queue to the exchange, and sets the quality of service
        /// </summary>
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

            /* It's creating a connection to the RabbitMQ server. */
            _connection = factory.CreateConnection();

            /* It's creating a channel. */
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("ms-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("ms-queue", false, false, false, null);
            _channel.QueueBind("ms-queue", "ms-exchange", "cqrs", null);
            _channel.BasicQos(0, 1, false);

            /* It's a delegate that is called when the connection is closed. */
            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogInformation($"Message queue connection shutting down...");
            };
        }

        /// <summary>
        /// The function is called when the service is started. It creates a consumer that listens to
        /// the queue and when a message is received, it calls the service to update the database
        /// </summary>
        /// <param name="CancellationToken">This is a token that can be used to cancel the task.</param>
        /// <returns>
        /// Task.CompletedTask
        /// </returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /* It's checking if the service is being stopped. */
            stoppingToken.ThrowIfCancellationRequested();

            /* It's creating a consumer that listens to the queue and when a message is received, it
            calls the service to update the database. */
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, args) =>
            {
                /* It's converting the message body to a string. */
                var messageString = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonConvert.DeserializeObject<MessageViewModel>(messageString);

                // Console.WriteLine($"{message.BookId}, {message.Title}, {message.Description}, {message.Author}");
                await _service.UpdateAsync(message);

                _channel.BasicAck(args.DeliveryTag, false);
            };

            /* It's telling the consumer to listen to the queue. */
            _channel.BasicConsume("ms-queue", false, consumer);
            return Task.CompletedTask;
        }

        /// <summary>
        /// The Dispose() function closes the channel and connection to the RabbitMQ server
        /// </summary>
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
