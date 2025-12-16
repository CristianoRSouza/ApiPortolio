namespace ApiEntregasMentoria.Data.Dto
{
    public class MatchDto
    {
        public int Id { get; set; }
        public string Team1Name { get; set; } = string.Empty;
        public string Team2Name { get; set; } = string.Empty;
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public DateTime MatchDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Team1Odd { get; set; }
        public decimal DrawOdd { get; set; }
        public decimal Team2Odd { get; set; }
    }
}
