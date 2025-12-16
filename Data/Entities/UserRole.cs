using System.ComponentModel.DataAnnotations;

namespace ApiEntregasMentoria.Data.Entities
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        
        public int? AssignedBy { get; set; }
        public virtual User? AssignedByUser { get; set; }
    }
}