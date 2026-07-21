using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DrugFormRepository : IDrugFormRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DrugForm>> GetAllAsync()
        {
            return await _context.DrugForms
                .OrderBy(x => x.FormCode)
                .ToListAsync();
        }

        public async Task<DrugForm?> GetByIdAsync(Guid formId)
        {
            return await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormId == formId);
        }

        public async Task<DrugForm?> GetByCodeAsync(string formCode)
        {
            return await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormCode.ToLower() == formCode.ToLower());
        }

        public async Task<DrugForm?> GetByNameAsync(string formName)
        {
            return await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormName.ToLower() == formName.ToLower());
        }

        public async Task<List<DrugForm>> GetByTypeAsync(string formType)
        {
            return await _context.DrugForms
                .Where(x => x.FormType != null && x.FormType.ToLower() == formType.ToLower())
                .OrderBy(x => x.FormCode)
                .ToListAsync();
        }

        public async Task<List<DrugForm>> GetActiveFormsAsync()
        {
            return await _context.DrugForms
                .Where(x => x.IsActive)
                .OrderBy(x => x.FormCode)
                .ToListAsync();
        }

        public async Task<(List<DrugForm> Items, int TotalCount)> GetFilteredAsync(DrugFormFilterDto filter)
        {
            var query = _context.DrugForms.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.FormCode))
                query = query.Where(x => x.FormCode.Contains(filter.FormCode));

            if (!string.IsNullOrEmpty(filter.FormName))
                query = query.Where(x => x.FormName.Contains(filter.FormName));

            if (!string.IsNullOrEmpty(filter.FormType))
                query = query.Where(x => x.FormType != null && x.FormType.Contains(filter.FormType));

            if (filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);


            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "formcode" => filter.SortDescending
                        ? query.OrderByDescending(x => x.FormCode)
                        : query.OrderBy(x => x.FormCode),
                    "formname" => filter.SortDescending
                        ? query.OrderByDescending(x => x.FormName)
                        : query.OrderBy(x => x.FormName),
                    "formtype" => filter.SortDescending
                        ? query.OrderByDescending(x => x.FormType)
                        : query.OrderBy(x => x.FormType)
                };
            }
            else
            {
                query = query.OrderBy(x => x.FormCode);
            }

            // Apply pagination
            var pageSize = filter.PageSize ?? 10;
            var page = filter.Page ?? 1;
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> InsertAsync(DrugForm drugForm)
        {
            drugForm.FormId = Guid.NewGuid();
            drugForm.CreatedOn = DateTime.UtcNow;
            drugForm.IsActive = true;

            await _context.DrugForms.AddAsync(drugForm);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertBulkAsync(List<DrugForm> drugForms)
        {
            foreach (var form in drugForms)
            {
                form.FormId = Guid.NewGuid();
                form.CreatedOn = DateTime.UtcNow;
                form.IsActive = true;
            }

            await _context.DrugForms.AddRangeAsync(drugForms);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(DrugForm drugForm)
        {
            var existing = await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormId == drugForm.FormId);

            if (existing == null)
                return false;

            existing.FormCode = drugForm.FormCode;
            existing.FormName = drugForm.FormName;
            existing.FormType = drugForm.FormType;
            existing.IsActive = drugForm.IsActive;
            existing.ModifiedBy = drugForm.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid formId)
        {
            var existing = await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormId == formId);

            if (existing == null)
                return false;

            // Check if form has drugs
            if (await HasDrugsAsync(formId))
                throw new InvalidOperationException("Cannot delete form that has associated drugs.");

            _context.DrugForms.Remove(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(Guid formId)
        {
            var existing = await _context.DrugForms
                .FirstOrDefaultAsync(x => x.FormId == formId);

            if (existing == null)
                return false;

            existing.IsActive = false;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByCodeAsync(string formCode, Guid? excludeId = null)
        {
            var query = _context.DrugForms
                .Where(x => x.FormCode.ToLower() == formCode.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.FormId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsByNameAsync(string formName, Guid? excludeId = null)
        {
            var query = _context.DrugForms
                .Where(x => x.FormName.ToLower() == formName.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.FormId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> HasDrugsAsync(Guid formId)
        {
            return await _context.Drugs
                .AnyAsync(x => x.IsActive);
        }

        public async Task<int> GetDrugCountByFormAsync(Guid formId)
        {
            return await _context.Drugs
                .CountAsync(x => x.IsActive);
        }

        public async Task<Dictionary<Guid, int>> GetDrugCountsByFormAsync(List<Guid> formIds)
        {
            var result = new Dictionary<Guid, int>();

            foreach (var formId in formIds)
            {
                var count = await GetDrugCountByFormAsync(formId);
                result[formId] = count;
            }

            return result;
        }
    }
}