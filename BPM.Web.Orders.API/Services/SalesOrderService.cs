using BPM.Web.API.Models.DTOs.Billing;
using BPM.Web.API.Models.DTOs.SalesOrder;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Services;
using Newtonsoft.Json;

namespace BPM.Web.Orders.API.Services
{
    /*  public class SalesOrderService : ISalesOrderService
      {
          private readonly IPurchaseOrderService _purchaseOrderService;
          private readonly IBillingService _billingService;
          private readonly ISalesOrderRepository _salesOrderRepository;
          private readonly ILogger<SalesOrderService> _logger;

          public SalesOrderService(ISalesOrderRepository salesOrderRepository, IPurchaseOrderService purchaseOrderService, IBillingService billingService, ILogger<SalesOrderService> logger)
          {
              _salesOrderRepository = salesOrderRepository;
              _purchaseOrderService = purchaseOrderService;
              _billingService = billingService;
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
                  if (!string.Equals(purchaseOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase) &&
                      !string.Equals(purchaseOrder.Status, "Verified", StringComparison.OrdinalIgnoreCase))
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
                      _logger.LogWarning("Sales Order not found with Id: {SalesOrderId}", id);
                      return null;
                  }

                  _logger.LogInformation("Sales Order found with Id: {SalesOrderId}", id);
                  return salesorder.ToDto();
              }
              catch (Exception ex)
              {
                  _logger.LogError(ex, "Error occurred while fetching Sales Order with Id: {SalesOrderId}", id);
                  throw;
              }
          }

          public async Task<SalesOrderDto> ProcessSalesOrderAsync(ProcessSalesOrderDto processSalesOrderDto, Guid currentUserId)
          {
              try
              {
                  _logger.LogInformation("Processing Sales Order with Id: {SalesOrderId}", processSalesOrderDto.SalesOrderId);

                  var salesOrder = await _salesOrderRepository.GetSalesOrderByIdAsync(processSalesOrderDto.SalesOrderId);

                  if (salesOrder == null)
                  {
                      _logger.LogWarning("Sales Order not found with Id: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                      throw new KeyNotFoundException("Sales Order not found.");
                  }

                  if (string.Equals(salesOrder.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                  {
                      _logger.LogWarning("Sales Order {SalesOrderId} is already approved.", processSalesOrderDto.SalesOrderId);
                      throw new InvalidOperationException("Sales Order is already approved.");
                  }

                  if (!string.Equals(processSalesOrderDto.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                  {
                      _logger.LogWarning("Invalid Sales Order status: {Status}", processSalesOrderDto.Status);
                      throw new InvalidOperationException("Only Approved status is allowed.");
                  }

                  var updatedSalesOrder = await _salesOrderRepository.ProcessSalesOrderAsync(
                      processSalesOrderDto.SalesOrderId,
                      processSalesOrderDto.Status);

                  updatedSalesOrder.ModifiedBy = currentUserId;
                  updatedSalesOrder.ModifiedOn = DateTime.UtcNow;

                  await _salesOrderRepository.UpdateSalesOrderAsync(updatedSalesOrder);

                  var createBillingDto = new CreateBillingDto
                  {
                      SalesOrderId = updatedSalesOrder.Id,
                      AdjustmentAmount = 0,
                      Remarks = "Billing generated for approved sales order"
                  };

                  await _billingService.CreateBillingAsync(createBillingDto, currentUserId);

                  _logger.LogInformation("Sales Order {SalesOrderId} approved successfully.", processSalesOrderDto.SalesOrderId);

                  return updatedSalesOrder.ToDto();
              }
              catch (Exception ex)
              {
                  _logger.LogError(ex, "Error occurred while processing Sales Order: {SalesOrderId}", processSalesOrderDto.SalesOrderId);
                  throw;
              }
          }
}*/
}
