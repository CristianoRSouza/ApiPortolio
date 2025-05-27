using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class MatchStatsMapping : IEntityTypeConfiguration<MatchStats>
    {
        public void Configure(EntityTypeBuilder<MatchStats> builder)
        {
            builder.ToTable("MatchStats");

            builder.HasKey(ms => ms.Id);

            builder.Property(ms => ms.Corners)
                .IsRequired();

            builder.Property(ms => ms.Fouls)
                .IsRequired();

            builder.Property(ms => ms.YellowCards)
                .IsRequired();

            builder.Property(ms => ms.RedCards)
                .IsRequired();

            builder.Property(ms => ms.Offsides)
                .IsRequired();

            builder.HasOne(ms => ms.Match)
                .WithOne() 
                .HasForeignKey<MatchStats>(ms => ms.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ms => ms.MatchId).IsUnique();
        }
    }
}

