using Microsoft.IdentityModel.Tokens;
using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NordesteFoodAPI.Modules.Auth.Infraestructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config) => _config = config;

        public string GenerateAccessToken(Guid userId, string username, string userRole)
        {
            var authClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, userRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"] ?? ""));
            var signedCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JWT:ExpirationTime"] ?? "15")),
                signingCredentials: signedCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
