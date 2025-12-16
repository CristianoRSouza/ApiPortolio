namespace ApiEntregasMentoria.Data.Entities
{
    public class Bet
    {
        public int Id { get; set; }
        public string TicketId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public string BetType { get; set; } = string.Empty;
        public string Selection { get; set; } = string.Empty;
        public decimal BetAmount { get; set; }
        public decimal SelectedOdd { get; set; }
        public decimal ResultAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
        public Match Match { get; set; } = null!;
    }
}
