using BPM.Web.Drug.API.Models.DTOs;
using BPM.Web.Drug.API.Models.Mappers;
using BPM.Web.Drug.API.Repositories;

namespace BPM.Web.Drug.API.Services
{
    public class DrugUomService : IDrugUomService
    {
        private readonly IDrugUomRepository _repository;
        private readonly ILogger<DrugUomService> _logger;

        public DrugUomService(IDrugUomRepository repository, ILogger<DrugUomService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DrugUomDto.ResponseDrugUomDto>> GetAllDrugUomsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all drug UOMs");

                var drugUoms = await _repository.GetAllDrugUomsAsync();

                return drugUoms.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all drug UOMs");
                throw;
            }
        }

        public async Task<DrugUomDto.ResponseDrugUomDto?> GetDrugUomByIdAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug UOM with Id {UomId}", uomId);

                var drugUom = await _repository.GetDrugUomByIdAsync(uomId);

                if (drugUom == null)
                {
                    _logger.LogWarning("Drug UOM not found with Id {UomId}", uomId);

                    return null;
                }

                return drugUom.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOM with Id {UomId}", uomId);
                throw;
            }
        }

        public async Task<List<DrugUomDto.ResponseDrugUomDto>> GetDrugUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving drug UOMs for DrugId {DrugId}", drugId);

                var drugUoms = await _repository.GetDrugUomsByDrugIdAsync(drugId);

                return drugUoms.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOMs for DrugId {DrugId}", drugId);
                throw;
            }
        }

        public async Task<DrugUomDto.ResponseDrugUomDto?> GetDrugUomByCodeAsync(Guid drugId, string uomCode)
        {
            try
            {
                _logger.LogInformation("Retrieving drug UOM with DrugId {DrugId} and UomCode {UomCode}", drugId, uomCode);

                var drugUom = await _repository.GetDrugUomByCodeAsync(drugId, uomCode);

                if (drugUom == null)
                {
                    _logger.LogWarning("Drug UOM not found with DrugId {DrugId} and UomCode {UomCode}", drugId, uomCode);

                    return null;
                }

                return drugUom.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving drug UOM with DrugId {DrugId} and UomCode {UomCode}", drugId, uomCode);
                throw;
            }
        }

        public async Task<List<DrugUomDto.ResponseDrugUomDto>> GetBaseUnitsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving base units for DrugId {DrugId}", drugId);

                var drugUoms = await _repository.GetBaseUnitsByDrugIdAsync(drugId);

                return drugUoms.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving base units for DrugId {DrugId}", drugId);
                throw;
            }
        }

        public async Task<List<DrugUomDto.ResponseDrugUomDto>> GetPurchaseUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving purchase UOMs for DrugId {DrugId}", drugId);

                var drugUoms = await _repository.GetPurchaseUomsByDrugIdAsync(drugId);

                return drugUoms.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase UOMs for DrugId {DrugId}", drugId);
                throw;
            }
        }

        public async Task<List<DrugUomDto.ResponseDrugUomDto>> GetSalesUomsByDrugIdAsync(Guid drugId)
        {
            try
            {
                _logger.LogInformation("Retrieving sales UOMs for DrugId {DrugId}", drugId);

                var drugUoms = await _repository.GetSalesUomsByDrugIdAsync(drugId);

                return drugUoms.ToDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sales UOMs for DrugId {DrugId}", drugId);
                throw;
            }
        }

        public async Task<bool> CreateDrugUomAsync(DrugUomDto.CreateDrugUomDto dto)
        {
            try
            {
                _logger.LogInformation("Creating drug UOM for DrugId {DrugId}", dto.DrugId);

                var exists = await _repository.DrugUomExistsAsync(dto.DrugId, dto.UomCode);

                if (exists)
                {
                    _logger.LogWarning("Drug UOM already exists with DrugId {DrugId} and UomCode {UomCode}", dto.DrugId, dto.UomCode);

                    return false;
                }

                var drugUom = dto.ToEntity();

                return await _repository.InsertDrugUomAsync(drugUom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating drug UOM");
                throw;
            }
        }

        public async Task<bool> UpdateDrugUomAsync(DrugUomDto.UpdateDrugUomDto dto)
        {
            try
            {
                _logger.LogInformation("Updating drug UOM with Id {UomId}", dto.UomId);

                var existing = await _repository.GetDrugUomByIdAsync(dto.UomId);

                if (existing == null)
                {
                    _logger.LogWarning("Drug UOM not found with Id {UomId}", dto.UomId);

                    return false;
                }

                var exists = await _repository.DrugUomExistsAsync(dto.DrugId, dto.UomCode, dto.UomId);

                if (exists)
                {
                    _logger.LogWarning("Drug UOM already exists with DrugId {DrugId} and UomCode {UomCode}", dto.DrugId, dto.UomCode);

                    return false;
                }

                var drugUom = dto.ToEntity();

                return await _repository.UpdateDrugUomAsync(drugUom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating drug UOM with Id {UomId}", dto.UomId);
                throw;
            }
        }

        public async Task<bool> DeleteDrugUomAsync(Guid uomId)
        {
            try
            {
                _logger.LogInformation("Deleting drug UOM with Id {UomId}", uomId);

                var existing = await _repository.GetDrugUomByIdAsync(uomId);

                if (existing == null)
                {
                    _logger.LogWarning("Drug UOM not found with Id {UomId}", uomId);

                    return false;
                }

                var hasChildUoms = await _repository.HasChildUomsAsync(uomId);

                if (hasChildUoms)
                {
                    _logger.LogWarning("Cannot delete drug UOM with Id {UomId} because child UOMs exist", uomId);

                    return false;
                }

                return await _repository.DeleteDrugUomAsync(uomId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting drug UOM with Id {UomId}", uomId);
                throw;
            }
        }
    }
}