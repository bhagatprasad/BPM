using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IBillingRepository
    {
        Task<Billing?> GetBillingBySalesOrderIdAsync(Guid salesOrderId);
        Task<Billing?> GetBillingByIdAsync(Guid id);
        Task<IEnumerable<Billing>> GetAllBillingAsync();
        Task<Billing>CreateBillingAsync(Billing billing);
    }
}
