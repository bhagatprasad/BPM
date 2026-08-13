using BPM.Web.API.Models.DTOs.Invoice;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBillingRepository _billingRepository;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IInvoiceRepository invoiceRepository, IBillingRepository billingRepository, ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _billingRepository = billingRepository;
            _logger = logger;
        }

        /*I received a request to create an Invoice. First, check whether the Billing exists. Then check whether an Invoice already 
         exists for that Billing. If everything is valid, convert the DTO to an Invoice entity, generate an Invoice Number, save 
         the Invoice through the repository, convert the saved entity to a response DTO, and return it.*/
        public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto, Guid currentUserId)
        {
            try
            {
                _logger.LogInformation("Creating Invoice from Billing: {BillingId}", createInvoiceDto.BillingId);

                var billing = await _billingRepository.GetBillingByIdAsync(createInvoiceDto.BillingId);

                if (billing == null)
                {
                    _logger.LogWarning("Billing not found: {BillingId}", createInvoiceDto.BillingId);
                    throw new KeyNotFoundException("Billing not found.");
                }

                var existingInvoice = await _invoiceRepository.GetInvoiceByBillingIdAsync(createInvoiceDto.BillingId);

                if (existingInvoice != null)
                {
                    _logger.LogWarning("Invoice already exists for Billing: {BillingId}", createInvoiceDto.BillingId);
                    throw new InvalidOperationException("Invoice already exists for this Billing.");
                }

                var invoice = createInvoiceDto.ToEntity(billing, currentUserId);

                invoice.InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

                var createdInvoice = await _invoiceRepository.CreateInvoiceAsync(invoice);

                _logger.LogInformation("Invoice {InvoiceNumber} created successfully from Billing {BillingId}", createdInvoice.InvoiceNumber, createInvoiceDto.BillingId);

                return createdInvoice.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Invoice from Billing: {BillingId}", createInvoiceDto.BillingId);
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoiceAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all invoices.");

                var invoices = await _invoiceRepository.GetAllInvoiceAsync();

                if (!invoices.Any())
                {
                    _logger.LogWarning("No invoices found.");
                    return Enumerable.Empty<InvoiceResponseDto>();
                }

                return invoices.Select(x => x.ToDto()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching invoices.");
                throw;
            }
        }

        public async Task<InvoiceResponseDto?> GetInvoiceByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Invoice with Id: {InvoiceId}", id);

                var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice not found with Id: {InvoiceId}", id);
                    return null;
                }

                return invoice.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Invoice with Id: {InvoiceId}", id);
                throw;
            }
        }

        public async Task<InvoiceResponseDto?> GetInvoiceByBillingIdAsync(Guid billingId)
        {
            try
            {
                _logger.LogInformation("Fetching Invoice for Billing: {BillingId}", billingId);

                var invoice = await _invoiceRepository.GetInvoiceByBillingIdAsync(billingId);

                if (invoice == null)
                {
                    _logger.LogWarning("Invoice not found for Billing: {BillingId}", billingId);
                    return null;
                }

                return invoice.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Invoice for Billing: {BillingId}", billingId);
                throw;
            }
        }
    }
}

