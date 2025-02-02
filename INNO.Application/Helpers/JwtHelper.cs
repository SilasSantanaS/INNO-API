using INNO.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace INNO.Application.Helpers
{
    public static class JwtHelper
    {
        public static string Secret = "asdfasdfa7sdf5a1998s7d1f87a1sd8f1a7s1df7a16sd5f1a8s1dfasdfasdfa4";

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public static string GenerateToken(User user, int durationInMinutes = 120)
        {
            return GenerateToken(
                [
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(CustomClaims.UserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.ProfileId.ToString("F")),
                    new Claim(CustomClaims.TenantId, user.TenantId.ToString()),
                ],
                durationInMinutes
            );
        }

        public static string GenerateToken(IDictionary<string, string> claims, int durationInMinutes = 120)
        {
            var claimList = new List<Claim>();

            foreach (var item in claims)
            {
                claimList.Add(new Claim(item.Key, item.Value));
            }

            return GenerateToken(claimList, durationInMinutes);
        }

        public static string GenerateToken(IEnumerable<Claim> claims, int durationInMinutes = 120)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    GetSecurityKey(),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static SymmetricSecurityKey GetSecurityKey()
        {
            var key = Encoding.ASCII.GetBytes(Secret);

            var result = new SymmetricSecurityKey(key);

            return result;
        }
    }

    public static class CustomClaims
    {
        public const string UserId = "user_id";
        public const string TenantId = "tenant_id";
        public const string InvitationHash = "inv_hash";
    }
}
