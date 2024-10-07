using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using user_GDPR.Models;

namespace user_GDPR.Repositories
{
    public class TokenRepository
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _expiryDuration;

        public TokenRepository(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expiryDuration = Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"]);
        }

        public string GenerateToken(Users user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            double expirationInMinutes = _expiryDuration;

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.lastName}"),
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "ADMIN" : "USER"),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("CreatedAt", user.CreatedAt.ToString("o"))
    };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
