﻿namespace ApiEntregasMentoria.Data.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public DateTime MatchDateTime { get; set; }

        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public ICollection<Market> Markets { get; set; } = new List<Market>();

        public MatchStats MatchStats { get; set; }
    }

}
