namespace BookCommand.Service.Models
{
    /* It's a class that holds the configuration for RabbitMQ */
    public class RabbitMQConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
    }
}
