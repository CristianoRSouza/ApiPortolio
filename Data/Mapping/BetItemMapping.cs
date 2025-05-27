using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class BetItemMapping : IEntityTypeConfiguration<BetItem>
    {
        public void Configure(EntityTypeBuilder<BetItem> builder)
        {
            builder.ToTable("BetItems");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(b => b.BetType)
                .IsRequired();

            builder.Property(b => b.Result)
                .IsRequired(false);

            builder.Property(b => b.BetDate)
                .IsRequired();

            builder.HasOne(b => b.User)
                .WithMany(u => u.BetItems)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Bet)
                .WithMany(b => b.Items)
                .HasForeignKey(b => b.BetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Market)
                .WithMany(m => m.BetItems)
                .HasForeignKey(b => b.MarketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
