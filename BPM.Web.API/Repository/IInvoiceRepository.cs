using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IInvoiceRepository
    {
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);

        Task<IEnumerable<Invoice>> GetAllInvoiceAsync();

        Task<Invoice?> GetInvoiceByIdAsync(Guid id);

        Task<Invoice?> GetInvoiceByBillingIdAsync(Guid billingId);
    }
}
