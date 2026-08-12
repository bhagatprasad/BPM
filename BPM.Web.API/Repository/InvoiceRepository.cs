using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            await _dbContext.Invoices.AddAsync(invoice);
            await _dbContext.SaveChangesAsync();
            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoiceAsync()
        {
            return await _dbContext.Invoices.Where(a=>a.IsActive).OrderByDescending(a=>a.InvoiceDate).ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByBillingIdAsync(Guid billingId)
        {
            return await _dbContext.Invoices.FirstOrDefaultAsync(a=>a.BillingId == billingId && a.IsActive);
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(Guid id)
        {
            return await _dbContext.Invoices.FirstOrDefaultAsync(a=>a.Id == id && a.IsActive);
        }
    }
}
