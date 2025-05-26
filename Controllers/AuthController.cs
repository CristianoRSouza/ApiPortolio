using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.JwtConfig;
using ApiEntregasMentoria.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiEntregasMentoria.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginDto user)
        {
            try
            {
                var result = _tokenService.GenerateToken(user);
                return Ok(result);
            }
            catch (WrongCredencials ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    Status = 401,
                    Message = ex.Message
                });
            }
            catch (UserNotFound ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 401,
                    Message = ex.Message
                });
            }
        }


    }
}
