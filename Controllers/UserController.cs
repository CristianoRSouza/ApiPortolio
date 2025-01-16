using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiEntregasMentoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _UserService;
        public UserController(IUserService userService)
        {
            _UserService = userService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _UserService.GetAllUser();
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Manager")]
        public async Task<UserDto> Get(int id)
        {
            return await _UserService.GetUser(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Post([FromBody] UserDto user)
        {
            await _UserService.AddUser(user);
        }

        [HttpPut("")]
        public async Task Put([FromBody] UserDto user)
        {
            await _UserService.UpdateUser(user);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _UserService.DeleteUser(id);
        }
    }
}
