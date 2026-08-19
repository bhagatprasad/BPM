using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRespository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRespository userRespository, ILogger<UserService> logger)
        {
            _userRespository = userRespository;
            _logger = logger;
        }
        public async Task<List<UserDto>> GetUsersListByDealerAsync(Guid dealerId)
        {
            try
            {
                _logger.LogInformation("Fetching users by dealer");

                var users = await _userRespository.GetUserListByDealerAsync(dealerId);

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found for dealer {DealerId}", dealerId);
                    return new List<UserDto>();
                }

                return users.ToUserDtoList(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users by dealer");
                throw;
            }
        }
        public async Task<List<UserDto>> GetUserListByDistributorAsync(Guid distributorId)
        {
            try
            {
                _logger.LogInformation("Fetching users by distributor");
                var users = await _userRespository.GetUserListByDistributorAsync(distributorId);

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found for distributor {DistributorId}", distributorId);
                    return new List<UserDto>();
                }
                return users.ToUserDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users by distributor");
                throw;
            }
        }


        public async Task<List<UserDto>> GetAllUsersListAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _userRespository.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found");

                    return new List<UserDto>();
                }

                return users.ToUserDtoList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users");
                throw;
            }
        }

        public async Task<bool> ActivateUserAync(UserActivateDto userActivateDto)
        {
            try
            {
                _logger.LogInformation("Activating user");

                var user = userActivateDto.ToEntity();

                var result = await _userRespository.ActivateUserAync(user);

                if (!result)
                {
                    _logger.LogWarning("Failed to activate user");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while activating user");
                throw;
            }
        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            try
            {
                _logger.LogInformation("Deactivating user");

                var user = userDeactivateDto.ToEntity();

                var result = await _userRespository.DeactivateUserAync(user);

                if (!result)
                {
                    _logger.LogWarning("Failed to deactivate user");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deactivating user");
                throw;
            }
        }

        public async Task<bool> InsertUserAsync(UserCreateDto user)
        {
            try
            {
                _logger.LogInformation("Creating user");
                if (!user.DealerId.HasValue && !user.DistributorId.HasValue)
                {
                    _logger.LogWarning("User must be associated with either a Dealer or Distributor");
                    return false;
                }
                if (user.DealerId.HasValue && user.DistributorId.HasValue)
                {
                    _logger.LogWarning("User cannot be associated with both Dealer and Distributor");
                    return false;
                }

                var newUser = user.ToEntity();

                var result = await _userRespository.InsertUserAsync(newUser);

                if (result == null)
                {
                    _logger.LogWarning("Failed to create user");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                throw;
            }
        }


        public async Task<UserDto> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user");

                userUpdateDto.UserId = userId;

                var user = userUpdateDto.ToEntity();

                var result = await _userRespository.UpdateUserInfoAsync(user);

                var dbUser = await _userRespository.GetUserByIdAsync(userId);

                if (dbUser == null)
                {
                    _logger.LogWarning("Failed to update user");
                    return null;
                }

                UserDto userUpdateResponse = dbUser.ToEntity();

                return userUpdateResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw;
            }
        }

        public async Task<bool> UpdateUserRoleAsync(UserRoleUpdateDto userRoleUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user role");

                var user = userRoleUpdateDto.ToEntity();

                var result = await _userRespository.UpdateUserRoleAsync(user);

                if (!result)
                {
                    _logger.LogWarning("Failed to update user role");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user role");
                throw;
            }
        }

        public async Task<bool> UpdateUserDealerAsync(UserDealerUpdateDto userDealerUpdateDto)
        {
            try
            {
                _logger.LogInformation("Updating user dealer");

                var user = userDealerUpdateDto.ToEntity();

                var result = await _userRespository.UpdateUserDealerAsync(user);

                if (!result)
                {
                    _logger.LogWarning("Failed to update user dealer");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user dealer");
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                _logger.LogInformation("Changing user password");

                var changedUser = userChangePasswordDto.ToEntity();

                var result = await _userRespository.ChangePasswordAsync(changedUser);

                if (!result)
                {
                    _logger.LogWarning("Failed to change user password");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing user password");
                throw;
            }
        }
    }
}