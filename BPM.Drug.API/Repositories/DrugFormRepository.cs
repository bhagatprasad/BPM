using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Entities;
using BPM.Web.Drug.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.Drug.API.Repositories
{
    public class DrugFormRepository : IDrugFormRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugFormRepository(ApplicationDbContext context) { _context = context; }

        public async Task<bool> DrugFormCodeExistsAsync(string formCode, Guid? excludeId = null)
        {
            var query = _context.DrugForms.Where(x => x.FormCode.ToLower() == formCode.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.FormId != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> DrugFormNameExistsAsync(string formName, Guid? excludeId = null)
        {
            var query = _context.DrugForms.Where(x => x.FormName.ToLower() == formName.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.FormId != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<List<DrugFormEntity>> GetActiveDrugFormsAsync()
        {
            return await _context.DrugForms.Where(e=>e.IsActive).OrderBy(f=>f.FormCode).ToListAsync();
        }

        public async Task<List<DrugFormEntity>> GetAllDrugFormsAsync()
        {
            return await _context.DrugForms.OrderBy(a=>a.FormCode).ToListAsync();
        }

        public async Task<int> GetDrugCountByFormAsync(Guid formId)
        {
            return await _context.Drugs.CountAsync(j => j.IsActive);
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

        public async Task<DrugFormEntity?> GetDrugFormByCodeAsync(string formCode)
        {
            return await _context.DrugForms.FirstOrDefaultAsync(b=>b.FormCode.ToLower() == formCode.ToLower());
        }

        public async Task<DrugFormEntity?> GetDrugFormByIdAsync(Guid formId)
        {
            return await _context.DrugForms.FirstOrDefaultAsync(a=>a.FormId == formId);
        }

        public async Task<DrugFormEntity?> GetDrugFormByNameAsync(string formName)
        {
            return await _context.DrugForms.FirstOrDefaultAsync(c=>c.FormName.ToLower() == formName.ToLower());
        }

        public async Task<List<DrugFormEntity>> GetDrugFormsByTypeAsync(string formType)
        {
            return await _context.DrugForms.Where(c=>c.FormType != null && c.FormType.ToLower() == formType.ToLower()).OrderBy(d=>d.FormCode).ToListAsync();
        }

        public async Task<(List<DrugFormEntity> Items, int TotalCount)> GetFilteredDrugFormsAsync(DrugFormDto.DrugFormFilterDto filter)
        {
            var query = _context.DrugForms.Include(a=>a.Drugs).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FormCode))
            {
                query = query.Where(x => x.FormCode.Contains(filter.FormCode));
            }

            if (!string.IsNullOrWhiteSpace(filter.FormName))
            {
                query = query.Where(x => x.FormName.Contains(filter.FormName));
            }

            if (!string.IsNullOrWhiteSpace(filter.FormType))
            {
                query = query.Where(x => x.FormType != null && x.FormType.Contains(filter.FormType));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            }

            if (filter.HasDrugs.HasValue)
            {
                query = filter.HasDrugs.Value
                    ? query.Where(x => x.Drugs.Any())
                    : query.Where(x => !x.Drugs.Any());
            }

            var totalCount = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
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
                        : query.OrderBy(x => x.FormType),

                    "isactive" => filter.SortDescending
                        ? query.OrderByDescending(x => x.IsActive)
                        : query.OrderBy(x => x.IsActive), 
                        _ => query.OrderBy(x => x.FormCode)
                };
            }
            else
            {
                query = query.OrderBy(x => x.FormCode);
            }

            var page = filter.Page.GetValueOrDefault(1);
            var pageSize = filter.PageSize.GetValueOrDefault(10);

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }

        public async Task<bool> HasDrugsAsync(Guid formId)
        {
            return await _context.Drugs.AnyAsync(i=>i.IsActive);
        }

        public async Task<bool> InsertBulkDrugFormsAsync(List<DrugFormEntity> drugForms)
        {
            foreach (var item in drugForms)
            {
                item.FormId=Guid.NewGuid();
                item.CreatedOn = DateTime.UtcNow;
                item.IsActive= true;
            }
            await _context.DrugForms.AddRangeAsync(drugForms);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertDrugFormAsync(DrugFormEntity drugForm)
        {
            drugForm.FormId = Guid.NewGuid();
            drugForm.CreatedOn = DateTime.UtcNow;
            drugForm.IsActive = true;

            await _context.DrugForms.AddAsync(drugForm);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> SoftDeleteDrugFormAsync(Guid formId)
        {
            var existingDrugForm= await _context.DrugForms.FirstOrDefaultAsync(h=>h.FormId==formId);
            if (existingDrugForm==null)
            {
                return false;
            }
            existingDrugForm.IsActive = false;
            existingDrugForm.ModifiedOn = DateTime.UtcNow;
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> UpdateDrugFormAsync(DrugFormEntity drugForm)
        {
           var existingDrugForm=await _context.DrugForms.FirstOrDefaultAsync(f=>f.FormId==drugForm.FormId);
            if (existingDrugForm==null)
            {
                return false;
            }
            existingDrugForm.FormCode= drugForm.FormCode;
            existingDrugForm.FormName= drugForm.FormName;
            existingDrugForm.FormType= drugForm.FormType;
            existingDrugForm.IsActive = drugForm.IsActive;
            existingDrugForm.ModifiedBy= drugForm.ModifiedBy;
            existingDrugForm.ModifiedOn= DateTime.UtcNow;

            return await _context.SaveChangesAsync()>0;
            
        }
    }
}
