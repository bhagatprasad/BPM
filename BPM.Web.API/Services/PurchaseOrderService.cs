using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using BPM.Web.API.Services;

namespace BPM.Web.API.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly ILogger<PurchaseOrderService> _logger;
        public PurchaseOrderService(IPurchaseOrderRepository repository, ILogger<PurchaseOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PurchaseOrder> CreatePurchaseOrderAsync(CreatePurchaseOrderDto createPurchaseOrderDto)
        {
            try
            {
                _logger.LogInformation("Creating Purchase Order.");

                // var purchaseOrder = PurchaseOrderMapper.ToEntity(purchaseOrderCreateDto);//its helper method
                var purchaseOrder = createPurchaseOrderDto.ToEntity();//purchaseorder

                var purchaseOrderItems = createPurchaseOrderDto.Items.Select(x => x.ToEntity()).ToList();//purchaseorderitems

                // Temporary PO Number Generation
                purchaseOrder.PONumber = $"PO-{DateTime.Now:yyyyMM}-{DateTime.Now.Ticks.ToString()[^4..]}";

                var result = await _repository.CreatePurchaseOrderAsync(purchaseOrder, purchaseOrderItems);

                _logger.LogInformation("Purchase Order created successfully. PO Number: {PONumber}", result.PONumber);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Purchase Order.");

                throw;
                //throw; preserves the original exception stack trace.
                //throw ex; resets the stack trace, making debugging harder.
            }
        }
    }
}