using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace StrategyManager.Infrastructure.Proxies.Clients
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private RabbitMQClientOptions options;
        private IConnection connection;
        private IModel channel;
        private bool disposed = false;

        public RabbitMQPublisher(IOptions<RabbitMQClientOptions> options)
        {
            this.options = options.Value;

            if (String.IsNullOrEmpty(this.options.HostName)) throw new ArgumentNullException(nameof(this.options.HostName));
            if (String.IsNullOrEmpty(this.options.UserName)) throw new ArgumentNullException(nameof(this.options.UserName));
            if (String.IsNullOrEmpty(this.options.Password)) throw new ArgumentNullException(nameof(this.options.Password));

            var factory = new ConnectionFactory()
            {
                HostName = this.options.HostName,
                UserName = this.options.UserName,
                Password = this.options.Password,
            };

            //connection = factory.CreateConnection();
            //channel = connection.CreateModel();
            //channel.QueueDeclare(
            //    queue: this.options.QueueName,
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false);
        }

        public void Publish(Event domainEvent)
        {
            //var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    var json = JsonSerializer.Serialize(domainEvent);
            //    var body = Encoding.UTF8.GetBytes(json);
            //    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            //};
            //var payload = new Payload()
            //{
            //    Message = message
            //};

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: options.QueueName,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(domainEvent)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                //Clean manageable resources
            };

            //Clean unmanageable resources
            channel.Close();
            connection.Close();
            channel.Dispose();
            connection.Dispose();

            disposed = true;
        }

        ~RabbitMQPublisher()
        {
            Dispose(false);
        }
    }
}
