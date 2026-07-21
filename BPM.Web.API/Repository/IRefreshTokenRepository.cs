namespace BPM.Web.API.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<bool> CreateAsync(RefreshToken token);

        Task<RefreshToken?> GetByTokenAsync(string refreshToken);

        Task<bool> UpdateAsync(RefreshToken token);

        Task<bool> RevokeAllAsync(Guid userId);
    }
}
