using BPM.Web.Orders.API.Models.DTOs;

namespace BPM.Web.Orders.API.Integrations
{
    public interface IBillingService
    {
        Task<BillingResponseDto> CreateBillingAsync(CreateBillingDto createBillingDto, Guid currentUserId);
        Task<IEnumerable<BillingResponseDto>> GetAllBillingAsync();
        Task<BillingResponseDto?> GetBillingByIdAsync(Guid id);
        Task<BillingResponseDto?> GetBillingBySalesOrderIdAsync(Guid salesOrderId);
    }
}
