using System.Security.Cryptography;

namespace BPM.Web.API.Helpers
{
    public static class RefreshTokenHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
