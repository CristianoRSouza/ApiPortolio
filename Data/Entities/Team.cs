﻿namespace ApiEntregasMentoria.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    }
}
