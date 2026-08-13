using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public RoleService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _repositoryFactory.SendAsync<List<RoleDto>>(
                HttpMethod.Get,
                "Role/get-all-roles");
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            return await _repositoryFactory.SendAsync<RoleDto>(
                HttpMethod.Get,
                $"Role/{id}");
        }

        public async Task<bool> CreateRoleAsync(CreateRoleDto dto)
        {
            return await _repositoryFactory.SendAsync<CreateRoleDto, bool>(
                HttpMethod.Post,
                "Role",
                dto);
        }

        public async Task<bool> UpdateRoleAsync(Guid id, UpdateRoleDto dto)
        {
            return await _repositoryFactory.SendAsync<UpdateRoleDto, bool>(
                HttpMethod.Put,
                $"Role/{id}",
                dto);
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            return await _repositoryFactory.SendAsync<bool>(
                HttpMethod.Delete,
                $"Role/{id}");
        }
    }
}