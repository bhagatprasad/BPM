using BPM.Web.Distributor.UI.Models.DTOs;

namespace BPM.Web.Distributor.UI.Services
{
    public class DrugService : IDrugService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        public DrugService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
        public Task<bool> DeleteDrugAsync(Guid drugId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DrugDto>> GetAllDrugsAsync()
        {
            return await _repositoryFactory.SendAsync<List<DrugDto>>(HttpMethod.Get, "drug/get-all-drugs");
        }

        public async Task<DrugDto?> GetDrugByIdAsync(Guid drugId)
        {
            return await _repositoryFactory.SendAsync<DrugDto?>(HttpMethod.Get, $"drug/get-drug-by-id/{drugId}");
        }

        public async Task<bool> InsertDrugAsync(CreateDrugDto drugDto)
        {
            return await _repositoryFactory.SendAsync<CreateDrugDto, bool>(HttpMethod.Post, "drug/create-drug", drugDto);
        }

        public async Task<bool> UpdateDrugAsync(UpdateDrugDto drugDto)
        {
            return await _repositoryFactory.SendAsync<UpdateDrugDto, bool>(HttpMethod.Put, $"drug/update-drug/{drugDto.DrugId}", drugDto);
        }
    }
}
