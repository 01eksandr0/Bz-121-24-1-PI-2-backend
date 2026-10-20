using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAgency.BLL.Models.Users;

namespace TravelAgency.WebAPI.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AccessToken Create(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtClaimNames.UserId, user.Id.ToString()),
                new Claim(JwtClaimNames.Name, user.FullName),
                new Claim(JwtClaimNames.Email, user.Email),
                new Claim(JwtClaimNames.Role, user.Role.ToString())
            };
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)), SecurityAlgorithms.HmacSha256);
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(_options.LifetimeMinutes);

            var token = new JwtSecurityToken(_options.Issuer, _options.Audience, claims,
                expires: expiresAt, signingCredentials: credentials);
            return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }
}
