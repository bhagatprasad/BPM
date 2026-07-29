using System.Text;
using System.Text.Json;
using BPM.Web.API.Models;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BPM.Web.API.RabbitMQ.Subscriber
{
    public class UserLoginHistorySubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMQSettings _settings;

        private IConnection _connection;
        private IChannel _channel;

        public UserLoginHistorySubscriber(
            IOptions<RabbitMQSettings> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _settings = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
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
                factory.Ssl.Enabled = true;

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
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var loginHistory = JsonSerializer.Deserialize<UserLoginHistory>(message);

                if (loginHistory != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var repository = scope.ServiceProvider
                        .GetRequiredService<IUserLoginHistoryRepository>();

                    await repository.AddAsync(loginHistory);
                }

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: _settings.UserLoginHistoryQueue,
                autoAck: false,
                consumer: consumer);
        }
    }
}