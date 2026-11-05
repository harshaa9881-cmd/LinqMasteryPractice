using Microsoft.IdentityModel.Tokens;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CampusPlacementManagement.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpiresAt) GenerateToken(
            ApplicationUser user,
            IList<string> roles)
        {
            var jwt = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.FullName
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email!
                )
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(
                double.Parse(jwt["ExpiryMinutes"]!)
            );

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                expiresAt
            );
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var generator = RandomNumberGenerator.Create();

            generator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}