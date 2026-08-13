using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class BillingRepository : IBillingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BillingRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
       
        public async Task<Billing> CreateBillingAsync(Billing billing)
        {
            await _dbContext.Billings.AddAsync(billing);
            await _dbContext.SaveChangesAsync();
            return billing;
        }

        public async Task<IEnumerable<Billing>> GetAllBillingAsync()
        {
            return await _dbContext.Billings.Where(a=>a.IsActive).OrderByDescending(a=>a.BillingDate).ToListAsync();
        }

        public async Task<Billing?> GetBillingByIdAsync(Guid id)
        {
            return await _dbContext.Billings.FirstOrDefaultAsync(b=>b.Id==id && b.IsActive);
        }

        public async Task<Billing?> GetBillingBySalesOrderIdAsync(Guid salesOrderId)
        {
            return await _dbContext.Billings.FirstOrDefaultAsync(c=>c.SalesOrderId==salesOrderId && c.IsActive);
        }
    }
}
