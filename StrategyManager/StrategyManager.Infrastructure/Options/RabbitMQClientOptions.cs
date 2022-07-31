namespace StrategyManager.Infrastructure.Options
{
    public class RabbitMQClientOptions
    {
        public string HostName { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string QueueName { get; set; } = String.Empty;
    }
}
