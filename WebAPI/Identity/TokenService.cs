using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Identity
{
    public class TokenService
    {
        public const string SECRETKEY = "2C90D6848AB74077A286EA344394663C";
        public const string ISSUER = "saltukz.com";
        public const string AUDIENCE = "saltukz.com";
        public static string GenerateToken(string username,string userID ="99",string role = "user")
        {
            byte[] key = Encoding.UTF8.GetBytes(SECRETKEY);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userID),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role,role)
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(ISSUER, AUDIENCE, claims, null, DateTime.Now.AddDays(30),credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
