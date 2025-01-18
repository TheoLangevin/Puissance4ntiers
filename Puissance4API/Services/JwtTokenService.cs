using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Puissance4Model.Models;

namespace Puissance4API.Services
{
    public class JwtTokenService
    {
        private readonly string _key;

        public JwtTokenService(string key)
        {
            _key = key;
        }

        public string GenerateJwtToken(Player player)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, player.Login),
                new Claim("PlayerId", player.Id.ToString()) // Inclure l'ID du joueur
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5263", // Émetteur du token
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Durée de validité
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}