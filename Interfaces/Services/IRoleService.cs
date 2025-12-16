using ApiEntregasMentoria.Data.Dto;

namespace ApiEntregasMentoria.Interfaces.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<bool> AssignRoleToUserAsync(int userId, int roleId, int assignedBy);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<List<UserWithRolesDto>> GetUsersWithRolesAsync();
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<bool> HasRoleAsync(int userId, string roleName);
    }
}