using BPM.Web.API.Models.Entities;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;

namespace BPM.Web.API.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _repository;

        public DrugService(IDrugRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Drug>> GetAllDrugsAsync()
        {
            return await _repository.GetAllDrugsAsync();
        }

        public async Task<Drug?> GetDrugByIdAsync(Guid drugId)
        {
            return await _repository.GetDrugByIdAsync(drugId);
        }

        public async Task<bool> InsertDrugAsync(Drug drug)
        {
            return await _repository.InsertDrugAsync(drug);
        }

        public async Task<bool> UpdateDrugAsync(Drug drug)
        {
            return await _repository.UpdateDrugAsync(drug);
        }

        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
            return await _repository.DeleteDrugAsync(drugId);
        }
    }
}
