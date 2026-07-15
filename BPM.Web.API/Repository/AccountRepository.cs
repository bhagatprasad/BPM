using BPM.Web.API.Helpes;
using BPM.Web.API.Models.Data;
using BPM.Web.API.Models.DTOs;
using BPM.Web.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BPM.Web.API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public string usedGenaratesTokenKey { get; }
        public AccountRepository(ApplicationDbContext context, string usedGenaratesTokenKey)
        {
            _context = context;
            this.usedGenaratesTokenKey = usedGenaratesTokenKey;
        }
        public async Task<User> AuthenticateAsync(string username)
        {
            var user = await _context.Users.Where(x => x.Email.ToLower().Trim() == username.ToLower().Trim() || x.Phone.Trim() == username.Trim()).FirstOrDefaultAsync();

            return user;
        }

        public async Task<AuthResponse> AuthenticateUser(string username, string password)
        {
            AuthResponse authResponse = new AuthResponse();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var dbUser = await _context.Users.Where(x => x.Email.ToLower().Trim() == username.ToLower().Trim() || x.Phone.Trim() == username.Trim()).FirstOrDefaultAsync();
                if (dbUser != null)
                {
                    var isValidUser = HashSalt.VerifyPassword(password, dbUser.PasswordHash, dbUser.PasswordSalt);

                    if (isValidUser)
                    {
                        var tokenhandler = new JwtSecurityTokenHandler();

                        var tokenkey = Encoding.ASCII.GetBytes(usedGenaratesTokenKey);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, username)

                            }),
                            Expires = DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(tokenkey),
                                SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenhandler.CreateToken(tokenDescriptor);

                        var writtoken = tokenhandler.WriteToken(token);

                        authResponse = new AuthResponse { JwtToken = writtoken };
                        authResponse.IsValidPassword = true;
                        authResponse.IsValidUser = true;
                        authResponse.IsActive = dbUser.IsActive;
                        authResponse.Message = string.Empty;
                    }
                    else
                    {
                        authResponse.Message = "Invalid Password";
                        authResponse.IsValidUser = true;
                        authResponse.IsValidPassword = false;
                    }
                }

                else
                {
                    authResponse.Message = "Invalid user";

                    authResponse.IsValidUser = false;
                }
            }
            return authResponse;
        }

        public async Task<AuthenticateResponseDto> GenarateUserClaims(AuthResponse authResponse)
        {
            try
            {
                var tokenkey = Encoding.ASCII.GetBytes(usedGenaratesTokenKey);
                var tokhand = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principle = tokhand.ValidateToken(authResponse.JwtToken,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    }, out securityToken);

                var jwttoken = securityToken as JwtSecurityToken;

                if (jwttoken != null && jwttoken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    string Username = principle.Identity.Name;
                    User user = _context.Users.Where(x => (x.Email == Username || x.Phone == Username) && x.IsActive).FirstOrDefault();
                    if (user != null)
                    {
                        AuthenticateResponseDto app = new AuthenticateResponseDto();
                        app.UserId = user.Id;
                        app.Email = user.Email;
                        app.FirstName = user.FirstName;
                        app.LastName = user.LastName;
                        app.Phone = user.Phone;
                        app.RoleId = user.RoleId;
                        return app;
                    }

                    return null;

                }
                else
                {
                    throw new SecurityTokenException("token invalid");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
