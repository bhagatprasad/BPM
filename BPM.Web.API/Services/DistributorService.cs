
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;
using BPM.Web.API.Services.Interfaces;

namespace BPM.Web.API.Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IDistributorRepository _distributorRepository;
        private readonly ILogger<DistributorService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public DistributorService(IDistributorRepository distributorRepository,
            ILogger<DistributorService> logger,
            IServiceProvider serviceProvider)
        {
            _distributorRepository = distributorRepository;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<DistributorDto> GetDistributorByIdAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching distributor by distributorId");
                var distributor = await _distributorRepository.GetDistributorByIdAsync(distributorId);

                if (distributor == null)
                {
                    _logger.LogWarning("Distributor not found with Id {DistributorId}", distributorId);
                    return null;
                }
                return distributor.ToDto();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while retrieving distributor with Id {DistributorId}", distributorId);
                throw;
            }
        }

        public async Task<List<DistributorDto>> GetDistributorListAsync()
        {
            try
            {
                _logger.LogInformation("fetching list of distributors");
                var distributors = await _distributorRepository.GetAllDistributorsAsync();
                return distributors.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("unable to fetch distributors");
                throw;
            }
        }

        public async Task<DistributorDto> InsertDistributorAsync(CreateDistributorDto distributorDto)
        {
            try
            {
                _logger.LogInformation("Creating Distributor");

                var distributorResponse = await _distributorRepository.InsertDistributorAsync(distributorDto.ToEntity());

                if (distributorResponse != null)
                {
                    if (distributorResponse.DistributorId != Guid.Empty || distributorResponse.DistributorId != null)
                    {
                        //create a distibutor user 


                        var _userService = _serviceProvider.GetRequiredService<IUserService>();

                        var _roleService = _serviceProvider.GetRequiredService<IRoleService>();

                        var _drugService = _serviceProvider.GetRequiredService<IDrugService>();

                        var _inventoryService = _serviceProvider.GetRequiredService<IInventoryService>();


                        var roles = await _roleService.GetAllRolesAsync();

                        var drgus = await _drugService.GetAllDrugsAsync();

                        await _userService.InsertUserAsync(distributorResponse.ToUserCreateDtoFromDistiutor(roles));


                        //creaete  werehouse for distibutor

                        var _wereHouseService = _serviceProvider.GetRequiredService<IWarehouseService>();


                        var warehouseResponse = await _wereHouseService.CreateAsync(distributorResponse.ToWarehouseCreateDtoFromDistributor());


                        //create a distibutor inventory

                        await _inventoryService.OnBoardInventoryAsync(distributorResponse.ToDto(), warehouseResponse, drgus);
                    }
                }
                return distributorResponse.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating distributor: {Message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                }
                throw;
            }
        }



        public async Task<DistributorDto?> UpdateDistributorAsync(Guid distributorId, UpdateDistributorDto updateDistributor)
        {
            try
            {
                _logger.LogInformation("Starting update for distributor with Id: {DistributorId}", distributorId);

                var dbDistributor = await _distributorRepository.GetDistributorByIdAsync(distributorId);
                if (dbDistributor == null)
                {
                    _logger.LogWarning("Distributor not found with Id: {DistributorId}", distributorId);
                    return null;
                }

                var updatedDbDistributor = updateDistributor.ToEntity(dbDistributor);
                var updateResult = await _distributorRepository.UpdateDistributorAsync(updatedDbDistributor);
                if (!updateResult)
                {
                    _logger.LogError("Failed to update distributor with Id: {DistributorId}", distributorId);
                    return null;
                }

                _logger.LogInformation("Distributor updated successfully with Id: {DistributorId}", distributorId);
                return updatedDbDistributor.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating distributor with Id: {DistributorId}", distributorId);
                throw;
            }
        }
        public async Task<bool> DeleteDistributorById(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Deleting distributor with Id: {DistributorId}", distributorId);

                var result = await _distributorRepository.DeleteDistributorAsync(distributorId);
                if (!result)
                {
                    _logger.LogError("Failed to delete distributor with Id: {DistributorId}", distributorId);
                    return false;
                }

                _logger.LogInformation("Distributor deleted successfully. Id: {DistributorId}", distributorId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting distributor with Id: {DistributorId}", distributorId);
                throw;
            }
        }
    }
}

