using BPM.Web.Identity.API.Models;
using BPM.Web.Identity.API.Models.Entities;
using BPM.Web.Identity.API.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BPM.Web.Identity.API.RabbitMQ.Subscriber
{
    public class RabbitMQSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMQSettings _settings;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQSubscriber(
            IOptions<RabbitMQSettings> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _settings = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
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
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _settings.UserLoginHistoryQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                var history = JsonSerializer.Deserialize<UserPasswordHistory>(message);

                if (history != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var repository = scope.ServiceProvider
                        .GetRequiredService<IUserPasswordHistoryRepository>();

                    await repository.AddAsync(history);
                }

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: _settings.PasswordHistoryQueue,
                autoAck: false,
                consumer: consumer);
        }
    }
}