namespace ApiEntregasMentoria.Data.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int ChampionshipId { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public decimal Team1Odd { get; set; }
        public decimal DrawOdd { get; set; }
        public decimal Team2Odd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Team Team1 { get; set; } = null!;
        public Team Team2 { get; set; } = null!;
        public Championship Championship { get; set; } = null!;
        public ICollection<Bet> Bets { get; set; } = new List<Bet>();
    }
}
