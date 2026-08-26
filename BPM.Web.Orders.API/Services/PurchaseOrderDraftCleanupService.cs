namespace BPM.Web.Orders.API.Services
{
   /* public class PurchaseOrderDraftCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PurchaseOrderDraftCleanupService> _logger;

        public PurchaseOrderDraftCleanupService(IServiceScopeFactory serviceScopeFactory, ILogger<PurchaseOrderDraftCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IPurchaseOrderRepository>();
                    var deletedCount = await repository.DeleteExpiredDraftPurchaseOrdersAsync();

                    if (deletedCount > 0)
                    {
                        _logger.LogInformation("Deleted {Count} expired Draft Purchase Orders.", deletedCount);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deleting expired Draft Purchase Orders.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }*/
}
