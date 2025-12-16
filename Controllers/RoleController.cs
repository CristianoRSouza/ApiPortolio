using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Interfaces.Services;
using System.Security.Claims;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult> AssignRole([FromBody] AssignRoleDto request)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var success = await _roleService.AssignRoleToUserAsync(request.UserId, request.RoleId, currentUserId);
            
            if (!success)
                return BadRequest("Role already assigned or invalid data");

            return Ok("Role assigned successfully");
        }

        [HttpDelete("remove/{userId}/{roleId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult> RemoveRole(int userId, int roleId)
        {
            var success = await _roleService.RemoveRoleFromUserAsync(userId, roleId);
            
            if (!success)
                return NotFound("User role not found");

            return Ok("Role removed successfully");
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<UserWithRolesDto>>> GetUsersWithRoles()
        {
            var users = await _roleService.GetUsersWithRolesAsync();
            return Ok(users);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<List<string>>> GetUserRoles(int userId)
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
    }
}