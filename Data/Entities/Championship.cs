namespace ApiEntregasMentoria.Data.Entities
{
    public class Championship
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
