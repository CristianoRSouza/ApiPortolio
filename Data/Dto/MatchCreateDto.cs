namespace ApiEntregasMentoria.Data.Dto
{
    public class MatchCreateDto
    {
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime MatchDate { get; set; }
    }

}
