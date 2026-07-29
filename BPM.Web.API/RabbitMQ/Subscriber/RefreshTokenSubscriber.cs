using System.Text;
using System.Text.Json;
using BPM.Web.API.Repository;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace BPM.Web.API.RabbitMQ.Subscriber
{
    public class RefreshTokenSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<RefreshTokenSubscriber> _logger;
        private IConnection _connection;
        private IChannel _channel;

        public RefreshTokenSubscriber(
            IOptions<RabbitMQSettings> options,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<RefreshTokenSubscriber> logger)
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
                    queue: _settings.RefreshTokenQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                await _channel.BasicQosAsync(0, 1, false);

                _logger.LogInformation("RefreshTokenSubscriber initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize RefreshTokenSubscriber");
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
                    _logger.LogInformation("Received refresh token message");

                    var refreshToken = JsonSerializer.Deserialize<RefreshToken>(message);

                    if (refreshToken != null)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var repository = scope.ServiceProvider
                            .GetRequiredService<IRefreshTokenRepository>();

                        await repository.CreateAsync(refreshToken);
                        _logger.LogInformation("Refresh token saved for UserId: {UserId}", refreshToken.UserId);
                    }

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing refresh token message");
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
                queue: _settings.RefreshTokenQueue,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("RefreshTokenSubscriber started consuming");

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
                _logger.LogInformation("RefreshTokenSubscriber stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping RefreshTokenSubscriber");
            }
            await base.StopAsync(cancellationToken);
        }
    }
}