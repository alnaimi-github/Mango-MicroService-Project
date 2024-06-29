using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mango.Service.AuthAPI.Models;
using Mango.Service.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Service.AuthAPI.Service
{
    public class JwtTokenGenerator: IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
            var claimList = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email!),
            new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id!),
            new Claim(JwtRegisteredClaimNames.Name,applicationUser.Name!),
            };
                claimList.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role,role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
              Issuer = _jwtOptions.Issuer,
              Audience = _jwtOptions.Audience,
              Subject = new ClaimsIdentity(claimList),
              Expires = DateTime.Now.AddDays(7),
              SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
