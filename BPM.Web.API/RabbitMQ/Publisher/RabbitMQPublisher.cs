using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BPM.Web.API.RabbitMQ.Publisher
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMQPublisher(IOptions<RabbitMQSettings> options)
        {
            _settings = options.Value;
        }


        public async Task PublishMessageAsync<T>(T message, string entityName)
        {

            string topicName = GetTopicName(entityName);

            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            if (_settings.UseSsl)
            {
                factory.Ssl.Enabled = true;
            }

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: topicName,
                durable: true, exclusive: false, autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: topicName, body: body);
        }


        public string GetTopicName(string entityName)
        {
            string topicName = string.Empty;

            if (!string.IsNullOrEmpty(entityName))
            {
                if (entityName == "PasswordHistoryQueue")
                {
                    topicName = _settings.PasswordHistoryQueue;
                }
                else if (entityName == "UserLoginHistoryQueue")
                {
                    topicName = _settings.UserLoginHistoryQueue;
                }
            }

            return topicName;
        }

    }
}