using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        public TokenService(IOptions<JwtSettings> settings, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _settings = settings.Value.Secret;
            _unitOfWork = unitOfWork;
        }

        public string GenerateToken(LoginDto user)
        {
            var userAuthenticated = VerifyUser(user);
            return ConfigJwt(userAuthenticated);
        }

        private User VerifyUser(LoginDto user)
        {
            var userEntity = _unitOfWork._UserRepository.GetByEmail(user.Email).Result;
            if (userEntity == null)
                throw new UserNotFound("Usuário não encontrado");

            var userAuthenticated = _passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, user.Password);
            if (userAuthenticated == PasswordVerificationResult.Failed)
                throw new WrongCredencials("Senha Incorreta!");

            return userEntity;
        }

        private string ConfigJwt(User userEntity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userEntity.Name),
                    new Claim(ClaimTypes.Role,userEntity.RolesToken.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
