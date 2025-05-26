namespace ApiEntregasMentoria.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }   
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreateAt { get; set; }
        public decimal? Saldo { get; set; }



        public ICollection<Bet> Bets { get; set; }

        public ICollection<BetItem> BetItems { get; set; }
        public RolesToken RolesToken { get; set; }
        public Adress Adress { get; set; }
        public int? AdressId { get; set; }
        public int? RoleId { get; set; }
    }
}
