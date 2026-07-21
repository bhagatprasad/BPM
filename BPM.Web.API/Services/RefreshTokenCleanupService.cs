using BPM.Web.API.Models.Data;
using Microsoft.EntityFrameworkCore;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<RefreshTokenCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var deleted = await context.RefreshTokens
                    .Where(x => x.ExpiresOn <= DateTime.UtcNow)
                    .ExecuteDeleteAsync(stoppingToken);

                _logger.LogInformation("{Count} expired refresh tokens deleted.", deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting expired refresh tokens.");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
