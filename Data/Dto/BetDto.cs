namespace ApiEntregasMentoria.Data.Dto
{
    public class BetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public string BetType { get; set; } = string.Empty;
        public decimal BetAmount { get; set; }
        public decimal SelectedOdd { get; set; }
        public decimal ResultAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BetMatchDto? Match { get; set; }
    }

    public class CreateBetDto
    {
        public int MatchId { get; set; }
        public string BetType { get; set; } = string.Empty;
        public decimal BetAmount { get; set; }
        public decimal SelectedOdd { get; set; }
    }

    public class BetMatchDto
    {
        public int Id { get; set; }
        public string Team1Name { get; set; } = string.Empty;
        public string Team2Name { get; set; } = string.Empty;
        public string ChampionshipName { get; set; } = string.Empty;
        public DateTime MatchDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
    }
}
