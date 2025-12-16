namespace ApiEntregasMentoria.Data.Entities
{
    public class Odd
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string BetType { get; set; } = string.Empty;
        public string Selection { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Match Match { get; set; } = null!;
    }
}
