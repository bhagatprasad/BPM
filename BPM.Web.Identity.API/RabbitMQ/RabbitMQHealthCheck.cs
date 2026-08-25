using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace BPM.Web.Identity.API.RabbitMQ
{
    public class RabbitMQHealthCheck : IHealthCheck
    {
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMQHealthCheck> _logger;

        public RabbitMQHealthCheck(IConnection connection, ILogger<RabbitMQHealthCheck> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (_connection != null && _connection.IsOpen)
                {
                    return HealthCheckResult.Healthy("RabbitMQ connection is healthy");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("RabbitMQ connection is not open");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ health check failed");
                return HealthCheckResult.Unhealthy("RabbitMQ health check failed", ex);
            }
        }
    }
}
