namespace StrategyManager.Infrastructure
{
    public class EnvVariableNameConstants
    {
        public const string EventStoreDbConnection = "EVENT_STORE_CONNECTION_STRING";
        public const string EventStoreDbName = "EVENT_STORE_DATABASE_NAME";
        public const string JwtSecret = "JWT_SECRET";
        public const string JwtExpirationTimeMinutes = "JWT_EXPIRATION_TIME_MINUTES";

        public const string SmtpHost = "SMTP_HOST";
        public const string SmtpPort = "SMTP_PORT";
        public const string SmtpLogin = "SMTP_LOGIN";
        public const string SmtpPassword = "SMTP_PASSWORD";
        public const string EmailFromAddress = "EMAIL_FROM_ADDRESS";

        public const string RabbitMQHostName = "RABBITMQ_HOST_NAME";
        public const string RabbitMQUserName = "RABBITMQ_USER_NAME";
        public const string RabbitMQPassword = "RABBITMQ_PASSWORD";
        public const string RabbitMQQueueName = "RABBITMQ_QUEUE_NAME";

        public const string BinanceApiUrl = "BINANCE_API_URL";
    }
}