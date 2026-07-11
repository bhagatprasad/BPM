using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Mappers;
using BPM.Web.API.Repository;

namespace BPM.Web.API.Services
{
    public class UserServiec : IUserServiec
    {
        private readonly IUserRespository _userRespository;
        public UserServiec(IUserRespository userRespository)
        {
            _userRespository = userRespository;
        }

        public async Task<bool> ActivateUserAync(UserActivateDto userActivateDto)
        {
            return await _userRespository.ActivateUserAync(userActivateDto.UserId, userActivateDto.IsActive, userActivateDto.ModifiedBy);
        }

        public async Task<bool> DeactivateUserAync(UserDeactivateDto userDeactivateDto)
        {
            return await _userRespository.DeactivateUserAync(userDeactivateDto.UserId,userDeactivateDto.ModifiedBy);
        }

        public async Task<bool> InsertUserAsync(UserCreateDto user)
        {
            var newUser = user.ToEntity();

            return await _userRespository.InsertUserAsync(newUser);
        }
    }
}
