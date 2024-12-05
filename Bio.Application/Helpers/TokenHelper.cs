using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Interfaces;
using Bio.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bio.Application.Helpers
{
    public class TokenHelper : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenHelper(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string CreateToken<TEntity>(TEntity entity) where TEntity : class
        {
            var claims = new List<Claim>();

            if (entity is User user)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Name, user.FullName));
                claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.JobSeeker)));
            }
            else if (entity is Company company)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, company.Id.ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, company.Email));
                claims.Add(new Claim(ClaimTypes.Name, company.FullName));
                claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.Company)));
            }
            else if (entity is Admin admin)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, admin.Email));
                claims.Add(new Claim(ClaimTypes.Name, admin.FullName));
                if (admin.Role == UserRole.SuperAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.SuperAdmin)));
                }
                else if (admin.Role == UserRole.Admin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, nameof(UserRole.Admin)));
                }
            }
            else
            {
                throw new ArgumentException("Unsupported entity type for token generation.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
