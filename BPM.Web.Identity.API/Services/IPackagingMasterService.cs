using BPM.Web.Identity.API.Models.Entities;

namespace BPM.Web.Identity.API.Services
{
    public interface IPackagingMasterService
    {
        Task<List<PackagingMaster>> GetAllAsync();

        Task<PackagingMaster?> GetByIdAsync(Guid packagingId);

        Task<bool> InsertAsync(PackagingMaster packaging);

        Task<bool> UpdateAsync(PackagingMaster packaging);

        Task<bool> DeleteAsync(Guid packagingId);
    }
}
