using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ultralinks.Domain.Models;

namespace Ultralinks.Application.Services
{
    public class TokenService
    {       
        public static object GenerateToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(ApiKey.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim("usuarioId", usuario.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                token = tokenString
            };
        }
    }

    public class ApiKey
    {
        public static string Secret = "fef7d8780b96a3ad92b087bdbff7d880f8de187db848f87d8e8af8f8de187db87b92b44b";
    }
}
