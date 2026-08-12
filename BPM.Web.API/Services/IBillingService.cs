using BPM.Web.API.Models.DTOs.Billing;

namespace BPM.Web.API.Services
{
    public interface IBillingService
    {
        Task<BillingResponseDto> CreateBillingAsync(CreateBillingDto createBillingDto, Guid currentUserId);
        Task<IEnumerable<BillingResponseDto>> GetAllBillingAsync();
        Task<BillingResponseDto?> GetBillingByIdAsync(Guid id);
        Task<BillingResponseDto?> GetBillingBySalesOrderIdAsync(Guid salesOrderId);
    }
}
