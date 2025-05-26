using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class BetMapping : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.ToTable("Bets");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BetDate).IsRequired();

            builder.HasOne(b => b.User)
                   .WithMany(u => u.Bets)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Items)
                   .WithOne(bi => bi.Bet)
                   .HasForeignKey(bi => bi.BetId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
