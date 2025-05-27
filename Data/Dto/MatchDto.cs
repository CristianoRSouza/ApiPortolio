using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Data.Dto
{
    public class MatchDto
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int Corners { get; set; }
        public int Fouls { get; set; }
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }
    }

}
