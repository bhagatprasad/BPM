using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPM.Web.API.Repository
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly ApplicationDbContext _context;

        public FeatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Feature>> GetAllAsync()
        {
            return await _context.Features
                .OrderBy(x => x.FeatureName)
                .ToListAsync();
        }

        public async Task<Feature?> GetByIdAsync(Guid featureId)
        {
            return await _context.Features
                .FirstOrDefaultAsync(x => x.FeatureId == featureId);
        }

        public async Task<Feature> AddAsync(Feature feature)
        {
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();

            return feature;
        }

        public async Task<Feature?> UpdateAsync(Feature feature)
        {
            var dbFeature = await _context.Features.FindAsync(feature.FeatureId);

            if (dbFeature == null)
                return null;

            dbFeature.FeatureName = feature.FeatureName;
            dbFeature.Code = feature.Code;
            dbFeature.Description = feature.Description;
            dbFeature.ModifiedBy = feature.ModifiedBy;
            dbFeature.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dbFeature;
        }

        public async Task<bool> DeleteAsync(Guid featureId)
        {
            var dbFeature = await _context.Features.FindAsync(featureId);

            if (dbFeature == null)
                return false;

            _context.Features.Remove(dbFeature);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
