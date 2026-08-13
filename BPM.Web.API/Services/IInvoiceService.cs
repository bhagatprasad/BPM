using BPM.Web.API.Models.DTOs.Invoice;

namespace BPM.Web.API.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto,Guid currentUserId);

        Task<IEnumerable<InvoiceResponseDto>> GetAllInvoiceAsync();

        Task<InvoiceResponseDto?> GetInvoiceByIdAsync(Guid id);

        Task<InvoiceResponseDto?> GetInvoiceByBillingIdAsync(Guid billingId);
    }
}
