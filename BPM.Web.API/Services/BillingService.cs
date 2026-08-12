using BPM.Web.API.Models.DTOs.Billing;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _billingRepository;
        private readonly ISalesOrderService _salesOrderService;
        private readonly ILogger<BillingService> _logger;

        public BillingService(
            IBillingRepository billingRepository,
            ISalesOrderService salesOrderService,
            ILogger<BillingService> logger)
        {
            _billingRepository = billingRepository;
            _salesOrderService = salesOrderService;
            _logger = logger;
        }

        public async Task<BillingResponseDto> CreateBillingAsync(CreateBillingDto createBillingDto, Guid currentUserId)
        {
            try
            {
                _logger.LogInformation("Creating Billing for Sales Order: {SalesOrderId}", createBillingDto.SalesOrderId);

                // 1. Validate Sales Order
                var salesOrder = await _salesOrderService.GetSalesOrderByIdAsync(createBillingDto.SalesOrderId);

                if (salesOrder == null)
                {
                    _logger.LogWarning("Sales Order not found: {SalesOrderId}", createBillingDto.SalesOrderId);
                    throw new KeyNotFoundException("Sales Order not found.");
                }

                // 2. Validate Sales Order Status
                if (!string.Equals(salesOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Sales Order {SalesOrderId} is not approved. Current Status: {Status}", createBillingDto.SalesOrderId, salesOrder.Status);
                    throw new InvalidOperationException("Billing can only be generated for an approved Sales Order.");
                }

                // 3. Check duplicate Billing
                var existingBilling = await _billingRepository.GetBillingBySalesOrderIdAsync(createBillingDto.SalesOrderId);

                if (existingBilling != null)
                {
                    _logger.LogWarning("Billing already exists for Sales Order: {SalesOrderId}", createBillingDto.SalesOrderId);
                    throw new InvalidOperationException("Billing already exists for this Sales Order.");
                }

                // 4. Convert Sales Order to Billing Entity
                var billing = createBillingDto.ToEntity(salesOrder, currentUserId);

                // 5. Generate Billing Number
                billing.BillingNumber = $"BILL-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

                // 6. Save Billing
                var createdBilling = await _billingRepository.CreateBillingAsync(billing);

                _logger.LogInformation("Billing {BillingNumber} created successfully for Sales Order: {SalesOrderId}", createdBilling.BillingNumber, createBillingDto.SalesOrderId);

                // 7. Return DTO
                return createdBilling.ToDto();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Sales Order not found while creating Billing: {SalesOrderId}", createBillingDto.SalesOrderId);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while creating Billing for Sales Order: {SalesOrderId}", createBillingDto.SalesOrderId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Billing for Sales Order: {SalesOrderId}", createBillingDto.SalesOrderId);
                throw;
            }
        }

        public async Task<IEnumerable<BillingResponseDto>> GetAllBillingAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all Billings.");

                var billings = await _billingRepository.GetAllBillingAsync();

                if (!billings.Any())
                {
                    _logger.LogWarning("No active Billings found.");
                    return Enumerable.Empty<BillingResponseDto>();
                }

                var result = billings.Select(x => x.ToDto()).ToList();

                _logger.LogInformation("Successfully fetched {Count} Billings.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all Billings.");
                throw;
            }
        }

        public async Task<BillingResponseDto?> GetBillingByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Billing with Id: {BillingId}", id);

                var billing = await _billingRepository.GetBillingByIdAsync(id);

                if (billing == null)
                {
                    _logger.LogWarning("Billing not found with Id: {BillingId}", id);
                    return null;
                }

                _logger.LogInformation("Billing found with Id: {BillingId}", id);

                return billing.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Billing with Id: {BillingId}", id);
                throw;
            }
        }

        public async Task<BillingResponseDto?> GetBillingBySalesOrderIdAsync(Guid salesOrderId)
        {
            try
            {
                _logger.LogInformation("Fetching Billing for Sales Order: {SalesOrderId}", salesOrderId);

                var billing = await _billingRepository.GetBillingBySalesOrderIdAsync(salesOrderId);

                if (billing == null)
                {
                    _logger.LogWarning("Billing not found for Sales Order: {SalesOrderId}", salesOrderId);
                    return null;
                }

                _logger.LogInformation("Billing found for Sales Order: {SalesOrderId}", salesOrderId);

                return billing.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Billing for Sales Order: {SalesOrderId}", salesOrderId);
                throw;
            }
        }
    }
}