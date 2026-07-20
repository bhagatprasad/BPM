using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Service
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