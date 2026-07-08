using System;

using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class DrugRepository : IDrugRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Drug>> GetAllDrugsAsync()
        {
            return await _context.Drugs
                .OrderBy(x => x.DrugName)
                .ToListAsync();
        }

        public async Task<Drug?> GetDrugByIdAsync(Guid drugId)
        {
            return await _context.Drugs
                .FirstOrDefaultAsync(x => x.DrugId == drugId);
        }

        public async Task<bool> InsertDrugAsync(Drug drug)
        {
            drug.DrugId = Guid.NewGuid();
            drug.CreatedOn = DateTime.UtcNow;
            drug.IsActive = true;

            await _context.Drugs.AddAsync(drug);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDrugAsync(Drug drug)
        {
            var existing = await _context.Drugs
                .FirstOrDefaultAsync(x => x.DrugId == drug.DrugId);

            if (existing == null)
                return false;

            existing.DrugCode = drug.DrugCode;
            existing.DrugName = drug.DrugName;
            existing.GenericName = drug.GenericName;
            existing.BrandName = drug.BrandName;
            existing.Manufacturer = drug.Manufacturer;
            existing.Category = drug.Category;
            existing.HSNCode = drug.HSNCode;
            existing.ScheduleType = drug.ScheduleType;
            existing.Packing = drug.Packing;
            existing.Strength = drug.Strength;
            existing.IsActive = drug.IsActive;
            existing.ModifiedBy = drug.ModifiedBy;
            existing.ModifiedOn = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
            var drug = await _context.Drugs
                .FirstOrDefaultAsync(x => x.DrugId == drugId);

            if (drug == null)
                return false;

            _context.Drugs.Remove(drug);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
