using Microsoft.EntityFrameworkCore;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Services;

namespace ApiEntregasMentoria.Services
{
    public class RoleService : IRoleService
    {
        private readonly MyContext _context;

        public RoleService(MyContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Where(r => r.IsActive)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId, int assignedBy)
        {
            var existingUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existingUserRole != null)
                return false;

            // Validate role exists
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);
            if (!roleExists)
                return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.UtcNow
            };

            _context.UserRoles.Add(userRole);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                return false;

            _context.UserRoles.Remove(userRole);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserWithRolesDto>> GetUsersWithRolesAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserWithRolesDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Nickname = u.Nickname,
                    Roles = u.UserRoles.Select(ur => new RoleDto
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        Description = ur.Role.Description,
                        IsActive = ur.Role.IsActive
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            return await _context.UserRoles
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);
        }
    }
}