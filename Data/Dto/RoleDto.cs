namespace ApiEntregasMentoria.Data.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class AssignRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class UserWithRolesDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new();
    }
}