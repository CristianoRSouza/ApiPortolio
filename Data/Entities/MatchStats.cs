namespace ApiEntregasMentoria.Data.Entities
{
    public class MatchStats
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int Corners { get; set; }
        public int Fouls { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Offsides { get; set; }
    }

}
