using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BPM.Web.Identity.API.RabbitMQ.Publisher
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
    {
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<RabbitMQPublisher> _logger;
        private IConnection _connection;
        private IChannel _channel;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private bool _disposed;
        private bool _initialized;

        public RabbitMQPublisher(
            IOptions<RabbitMQSettings> options,
            ILogger<RabbitMQPublisher> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        private async Task EnsureConnectionAsync()
        {
            if (_initialized && _connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                return;

            await _connectionLock.WaitAsync();
            try
            {
                if (_initialized && _connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
                    return;

                // Dispose old resources
                if (_channel != null)
                {
                    try { await _channel.CloseAsync(); } catch { }
                    try { await _channel.DisposeAsync(); } catch { }
                }
                if (_connection != null)
                {
                    try { await _connection.CloseAsync(); } catch { }
                    try { await _connection.DisposeAsync(); } catch { }
                }

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

                _initialized = true;
                _logger.LogInformation("RabbitMQ Publisher connected successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize RabbitMQ connection");
                _initialized = false;
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task<IChannel> GetChannelAsync()
        {
            await EnsureConnectionAsync();
            return _channel;
        }

        public async Task PublishMessageAsync<T>(T message, string entityName)
        {
            try
            {
                string queueName = GetQueueName(entityName);
                await PublishMessageInternalAsync(message, queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to {EntityName}", entityName);
                throw;
            }
        }

        public async Task PublishMessageWithRetryAsync<T>(T message, string entityName, int maxRetries = 3)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    await PublishMessageAsync(message, entityName);
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex, "Publish attempt {RetryCount} failed for {EntityName}. Retrying...", retryCount, entityName);

                    if (retryCount >= maxRetries)
                    {
                        _logger.LogError(ex, "All {MaxRetries} attempts failed for {EntityName}", maxRetries, entityName);
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)));

                    // Reset connection on failure
                    _initialized = false;
                }
            }
        }

        private async Task PublishMessageInternalAsync<T>(T message, string queueName)
        {
            var channel = await GetChannelAsync();

            // Ensure queue exists
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            // Publish the message (without confirms)
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published message to queue {QueueName}", queueName);
        }

        public string GetQueueName(string entityName)
        {
            return entityName switch
            {
                "PasswordHistoryQueue" => _settings.PasswordHistoryQueue,
                "UserLoginHistoryQueue" => _settings.UserLoginHistoryQueue,
                "RefreshTokenQueue" => _settings.RefreshTokenQueue,
                _ => throw new ArgumentException($"Unknown entity name: {entityName}")
            };
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            try
            {
                if (_channel != null)
                {
                    try { await _channel.CloseAsync(); } catch { }
                    try { await _channel.DisposeAsync(); } catch { }
                }
                if (_connection != null)
                {
                    try { await _connection.CloseAsync(); } catch { }
                    try { await _connection.DisposeAsync(); } catch { }
                }
                _connectionLock?.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error disposing RabbitMQ resources");
            }

            _disposed = true;
        }

        public void Dispose()
        {
            // Synchronous dispose for backwards compatibility
            if (_disposed) return;

            try
            {
                if (_channel != null)
                {
                    try { _channel.CloseAsync().GetAwaiter().GetResult(); } catch { }
                    try { _channel.DisposeAsync().AsTask().GetAwaiter().GetResult(); } catch { }
                }
                if (_connection != null)
                {
                    try { _connection.CloseAsync().GetAwaiter().GetResult(); } catch { }
                    try { _connection.DisposeAsync().AsTask().GetAwaiter().GetResult(); } catch { }
                }
                _connectionLock?.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error disposing RabbitMQ resources");
            }

            _disposed = true;
        }
    }
}