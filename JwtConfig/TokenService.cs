using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiEntregasMentoria.JwtConfig
{
    public class TokenService
    {
        private readonly string _settings;
        private readonly IRepositoryUser _repositoryUser;

        public TokenService(IOptions<JwtSettings> settings, IRepositoryUser repositoryUser)
        {
            _settings = settings.Value.Secret;
            _repositoryUser = repositoryUser;
        }
        public string GenerateToken(UserDto user)
        {
            var result = _repositoryUser.ValidateUser(user.Email, user.Password).Result;
            if (result == null)
                throw new Exception("User not found");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, result.Name),
                    new Claim(ClaimTypes.Role,result.RolesToken.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
