namespace ApiEntregasMentoria.Data.Entities
{
    public class RolesToken
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
