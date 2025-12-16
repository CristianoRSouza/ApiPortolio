namespace ApiEntregasMentoria.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Cpf { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Bet> Bets { get; set; } = new List<Bet>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
