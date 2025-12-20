using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MyContext _context;
        private readonly IConfiguration _configuration;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IRoleService _roleService;

        public AuthController(MyContext context, IConfiguration configuration, IValidator<RegisterDto> registerValidator, IRoleService roleService)
        {
            _context = context;
            _configuration = configuration;
            _registerValidator = registerValidator;
            _roleService = roleService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            if (!user.IsActive)
            {
                return BadRequest(new { message = "Account is inactive" });
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = await GenerateJwtTokenAsync(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nickname = user.Nickname,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Cpf = user.Cpf,
                    Balance = user.Balance,
                    IsVerified = user.IsVerified,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin
                }
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterDto request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (await _context.Set<User>().AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var user = new User
            {
                Email = request.Email,
                Nickname = request.Nickname,
                FullName = request.FullName,
                Phone = request.Phone,
                Cpf = request.Cpf,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Balance = 0,
                IsVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            // Assign default User role
            var roleAssigned = await _roleService.AssignRoleToUserAsync(user.Id, 1, user.Id); // Role ID 1 = User
            if (!roleAssigned)
            {
                return BadRequest("Failed to assign default role. Please ensure the role exists.");
            }

            var token = await GenerateJwtTokenAsync(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nickname = user.Nickname,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Cpf = user.Cpf,
                    Balance = user.Balance,
                    IsVerified = user.IsVerified,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin
                }
            });
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSecret = _configuration["Jwt:Key"] ?? "SoccerBetSecretKey123456789012345678901234567890123456789012345678901234567890";
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Nickname)
            };

            // Get user roles from database
            var userRoles = await _roleService.GetUserRolesAsync(user.Id);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }
}
