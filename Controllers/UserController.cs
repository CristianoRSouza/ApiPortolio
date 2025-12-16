using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEntregasMentoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUser();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                if (user == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Status = 404,
                        Message = $"Usuário com ID {id} não foi encontrado"
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserDto user)
        {
            try
            {
                await _userService.AddUser(user);
                return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Put([FromBody] UserDto user)
        {
            try
            {
                var existingUser = await _userService.GetUser(user.Id);

                if (existingUser == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Status = 404,
                        Message = $"Usuário com ID {user.Id} não encontrado"
                    });
                }

                await _userService.UpdateUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                if (user == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Status = 404,
                        Message = $"Usuário com ID {id} não encontrado"
                    });
                }

                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Status = 500,
                    Message = ex.Message
                });
            }
        }
    }
}
