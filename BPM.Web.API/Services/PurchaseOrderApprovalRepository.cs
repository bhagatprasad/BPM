using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repositories
{
    public class PurchaseOrderApprovalRepository : IPurchaseOrderApprovalRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrderApproval> CreateApprovalAsync(PurchaseOrderApproval approval)
        {
            await _context.PurchaseOrderApprovals.AddAsync(approval);
            await _context.SaveChangesAsync();

            return approval;
        }

        public async Task<List<PurchaseOrderApproval>> CreateApprovalsAsync(List<PurchaseOrderApproval> approvals)
        {
            await _context.PurchaseOrderApprovals.AddRangeAsync(approvals);
            await _context.SaveChangesAsync();

            return approvals;
        }

        public async Task<List<PurchaseOrderApproval>> GetApprovalsByPurchaseOrderIdAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderApprovals
                .Where(x => x.PurchaseOrderId == purchaseOrderId)
                .OrderBy(x => x.ApprovalLevel)
                .ToListAsync();
        }

        public async Task<PurchaseOrderApproval?> GetApprovalByIdAsync(Guid approvalId)
        {
            return await _context.PurchaseOrderApprovals
                .FirstOrDefaultAsync(x => x.Id == approvalId);
        }

        public async Task<PurchaseOrderApproval> UpdateApprovalAsync(PurchaseOrderApproval approval)
        {
            _context.PurchaseOrderApprovals.Update(approval);
            await _context.SaveChangesAsync();

            return approval;
        }

        public async Task<List<User>> GetActiveApproversAsync()
        {
            return await _context.Users
                .Include(x => x.Role)
                .Where(x =>
                    x.IsActive &&
                    x.Role != null &&
                    x.Role.Code == "ADMIN")
                .OrderBy(x => x.FirstName)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> SubmitPurchaseOrderWithApprovalsAsync(PurchaseOrder purchaseOrder, List<PurchaseOrderApproval> approvalRecords)
        {
            // Create the EF Core execution strategy so the transaction can be retried safely.
            var strategy = _context.Database.CreateExecutionStrategy();

            // Execute the Purchase Order update and approval creation as one retriable unit.
            return await strategy.ExecuteAsync(async () =>
            {
                // Begin the database transaction inside the execution strategy.
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Update the Purchase Order status and audit information.
                    _context.PurchaseOrders.Update(purchaseOrder);

                    // Add all generated approval records.
                    await _context.PurchaseOrderApprovals.AddRangeAsync(approvalRecords);

                    // Save the Purchase Order and approval records together.
                    await _context.SaveChangesAsync();

                    // Commit the transaction after all database operations succeed.
                    await transaction.CommitAsync();

                    // Return the submitted Purchase Order.
                    return purchaseOrder;
                }
                catch
                {
                    // Roll back the transaction when any database operation fails.
                    await transaction.RollbackAsync();

                    // Re-throw the exception so the service/controller can handle it.
                    throw;
                }
            });
        }
    }
}