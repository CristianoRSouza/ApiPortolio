using ApiEntregasMentoria.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEntregasMentoria.Data.Entities
{
    public class RolesToken
    {
        public int Id { get; set; }
        public string Role { get; set; }

        [NotMapped]
        public RoleType RoleEnum 
        { 
            get => Enum.Parse<RoleType>(Role);
            set => Role = value.ToString(); 
        }
        public ICollection<User> Users { get; set; }
    }
}
