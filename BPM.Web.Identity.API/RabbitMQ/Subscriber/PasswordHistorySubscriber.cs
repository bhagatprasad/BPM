using System.Text;
using System.Text.Json;
using BPM.Web.Identity.API.Models.Entities;
using BPM.Web.Identity.API.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BPM.Web.Identity.API.RabbitMQ.Subscriber
{
    public class PasswordHistorySubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<PasswordHistorySubscriber> _logger;
        private IConnection _connection;
        private IChannel _channel;
        private bool _initialized;

        public PasswordHistorySubscriber(
            IOptions<RabbitMQSettings> options,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PasswordHistorySubscriber> logger)
        {
            _settings = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQAsync();
            await StartConsumingAsync(stoppingToken);
        }

        private async Task InitializeRabbitMQAsync()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.Host,
                    Port = _settings.Port,
                    UserName = _settings.Username,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                    RequestedHeartbeat = TimeSpan.FromSeconds(30)
                };

                if (_settings.UseSsl)
                {
                    factory.Ssl.Enabled = true;
                    factory.Ssl.ServerName = _settings.Host;
                }

                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                await _channel.QueueDeclareAsync(
                    queue: _settings.PasswordHistoryQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Set prefetch count for better load balancing
                await _channel.BasicQosAsync(0, 1, false);

                _initialized = true;
                _logger.LogInformation("PasswordHistorySubscriber initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize PasswordHistorySubscriber");
                throw;
            }
        }

        private async Task StartConsumingAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    _logger.LogInformation("Received password history message");

                    var history = JsonSerializer.Deserialize<UserPasswordHistory>(message);

                    if (history != null)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var repository = scope.ServiceProvider
                            .GetRequiredService<IUserPasswordHistoryRepository>();

                        await repository.AddAsync(history);
                        _logger.LogInformation("Password history saved for UserId: {UserId}", history.UserId);
                    }

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing password history message");
                    try
                    {
                        await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);
                    }
                    catch (Exception nackEx)
                    {
                        _logger.LogError(nackEx, "Error sending NACK");
                    }
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _settings.PasswordHistoryQueue,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("PasswordHistorySubscriber started consuming");

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_channel != null && _channel.IsOpen)
                {
                    await _channel.CloseAsync();
                }
                if (_connection != null && _connection.IsOpen)
                {
                    await _connection.CloseAsync();
                }
                _logger.LogInformation("PasswordHistorySubscriber stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping PasswordHistorySubscriber");
            }
            await base.StopAsync(cancellationToken);
        }
    }
}