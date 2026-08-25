using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Mappers;
using BPM.Web.Drug.API.Repositories;

namespace BPM.Web.Drug.API.Services
{
    public class DrugService : IDrugService
    {
        private readonly IDrugRepository _repository;
        private readonly ILogger<DrugService> _logger;

        public DrugService(IDrugRepository repository, ILogger<DrugService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugDto.ResponseDrugDto>> GetAllDrugsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drugs");

                var drugs = await _repository.GetAllDrugsAsync();

                return drugs.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drugs");
                throw;
            }
        }

        public async Task<DrugDto.ResponseDrugDto?> GetDrugByIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug with Id {DrugId}", drugId);

                var drug = await _repository.GetDrugByIdAsync(drugId);

                if (drug == null)
                {
                    _logger.LogWarning("Drug not found with Id {DrugId}", drugId);
                    return null;
                }

                return drug.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug with Id {DrugId}", drugId);
                throw;
            }
        }

        public async Task<bool> CreateDrugAsync(DrugDto.CreateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug");

                var drug = dto.ToEntity();

                return await _repository.InsertDrugAsync(drug);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug");
                throw;
            }
        }

        public async Task<bool> UpdateDrugAsync(DrugDto.UpdateDrugDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug with Id {DrugId}", dto.DrugId);

                var drug = dto.ToEntity();

                return await _repository.UpdateDrugAsync(drug);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug with Id {DrugId}", dto.DrugId);
                throw;
            }
        }

        public async Task<bool> DeleteDrugAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Deleting drug with Id {DrugId}", drugId);

                return await _repository.DeleteDrugAsync(drugId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug with Id {DrugId}", drugId);
                throw;
            }
        }
    }
}