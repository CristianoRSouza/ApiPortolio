using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class MatchMapping : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.MatchDateTime).IsRequired();
            builder.Property(m => m.HomeGoals);
            builder.Property(m => m.AwayGoals);

            builder.HasOne(m => m.HomeTeam)
                   .WithMany(t => t.HomeMatches)
                   .HasForeignKey(m => m.HomeTeamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AwayTeam)
                   .WithMany(t => t.AwayMatches)
                   .HasForeignKey(m => m.AwayTeamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.MatchStats)
                   .WithOne(ms => ms.Match)
                   .HasForeignKey<MatchStats>(ms => ms.MatchId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
