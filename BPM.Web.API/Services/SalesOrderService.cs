using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using Newtonsoft.Json;

namespace BPM.Web.API.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ILogger<SalesOrderService> _logger;

        public SalesOrderService(ISalesOrderRepository salesOrderRepository, IPurchaseOrderService purchaseOrderService, ILogger<SalesOrderService> logger)
        {
            _salesOrderRepository = salesOrderRepository;
            _purchaseOrderService = purchaseOrderService;
            _logger = logger;
        }

        public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrderAsync()
        {
            try
            {
                _logger.LogInformation("Getting all sales orders");
                var salesorders = await _salesOrderRepository.GetAllSalesOrderAsync();
                return salesorders.Select(SalesOrderMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all sales orders");
                throw;
            }
        }

        public async Task<IEnumerable<SalesOrderDto>> GetSalesOrderByDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Getting sales orders for DealerId: {DealerId}", dealerId);
                var salesorder = await _salesOrderRepository.GetSalesOrderByDealer(dealerId);
                return salesorder.Select(SalesOrderMapper.ToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occurred while getting sales order for DealerId : {dealerId}", dealerId);
                throw;
            }
        }

        public async Task<SalesOrderDto> CreateSalesOrderFromPurchaseOrderAsync(Guid purchaseOrderId, Guid createdBy)
        {
            try
            {
                _logger.LogInformation("Creating Sales Order from Purchase Order: {PurchaseOrderId}", purchaseOrderId);

                // 1. Get Purchase Order with Items
                var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(purchaseOrderId);

                // 2. Validate Purchase Order exists
                if (purchaseOrder == null)
                {
                    _logger.LogWarning("Purchase Order not found: {PurchaseOrderId}", purchaseOrderId);

                    throw new KeyNotFoundException("Purchase Order not found.");
                }

                // 3. Validate Purchase Order status
                if (!string.Equals(purchaseOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Purchase Order {PurchaseOrderId} is not approved. Current status: {Status}", purchaseOrderId, purchaseOrder.Status);

                    throw new InvalidOperationException("Only approved Purchase Orders can be converted to Sales Orders.");
                }

                var salesOrderAfterMapping = purchaseOrder.ToSalesOrderFromPurchaseOrder(createdBy);

                var order = JsonConvert.SerializeObject(salesOrderAfterMapping);


                var createdSalesOrder = await _salesOrderRepository.CreateSalesOrderAsync(salesOrderAfterMapping);

                _logger.LogInformation("Sales Order {SONumber} created successfully from Purchase Order {PurchaseOrderId}", createdSalesOrder.SONumber, purchaseOrderId);

                // 8. Return DTO
                return createdSalesOrder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Sales Order from Purchase Order: {PurchaseOrderId}", purchaseOrderId);

                throw;
            }
        }

        public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Sales Order with Id: {SalesOrderId}", id);

                var salesorder = await _salesOrderRepository.GetSalesOrderByIdAsync(id);

                if (salesorder == null)
                {
                    _logger.LogWarning("Sales Order not found with Id: {SalesOrderId}",id);
                     return null;
                }

                _logger.LogInformation("Sales Order found with Id: {SalesOrderId}",id);
                return salesorder.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Sales Order with Id: {SalesOrderId}",id);
                throw;
            }
        }
    }
}
