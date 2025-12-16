namespace ApiEntregasMentoria.Data.Entities
{
    public class Adress
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
