using BPM.Web.API.Models.Entities;

namespace BPM.Web.API.Repository
{
    public interface IUserRespository
    {
        Task<bool> InsertUserAsync(User user);
        Task<bool> ActivateUserAync(Guid userId, bool isActive,Guid modifiedBy);
        Task<bool> DeactivateUserAync(Guid userId,Guid modifiedBy);
    }
}
