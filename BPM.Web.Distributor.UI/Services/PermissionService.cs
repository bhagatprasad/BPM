using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public PermissionService(
            IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            return await _repositoryFactory.SendAsync<List<PermissionDto>>(
                HttpMethod.Get,
                "Permission");
        }

        public async Task<PermissionDto?> GetByIdAsync(Guid permissionId)
        {
            return await _repositoryFactory.SendAsync<PermissionDto>(
                HttpMethod.Get,
                $"Permission/{permissionId}");
        }

        public async Task<PermissionDto?> CreateAsync(
            PermissionCreateDto dto)
        {
            return await _repositoryFactory.SendAsync<
                PermissionCreateDto,
                PermissionDto>(
                HttpMethod.Post,
                "Permission",
                dto);
        }

        public async Task<PermissionDto?> UpdateAsync(
            Guid permissionId,
            PermissionUpdateDto dto)
        {
            return await _repositoryFactory.SendAsync<
                PermissionUpdateDto,
                PermissionDto>(
                HttpMethod.Put,
                $"Permission/{permissionId}",
                dto);
        }

        public async Task<bool> DeleteAsync(Guid permissionId)
        {
            await _repositoryFactory.SendAsync<string>(
                HttpMethod.Delete,
                $"Permission/{permissionId}");

            return true;
        }
    }
}