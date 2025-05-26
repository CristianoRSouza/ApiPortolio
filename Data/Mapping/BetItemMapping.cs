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
            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.Amount)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(bi => bi.BetType)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(bi => bi.Result)
                   .HasMaxLength(50);

            builder.Property(bi => bi.BetDate)
                   .IsRequired();

            builder.HasOne(bi => bi.User)
                   .WithMany(u => u.BetItems)
                   .HasForeignKey(bi => bi.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // Evita ciclo

            builder.HasOne(bi => bi.Match)
                   .WithMany(m => m.BetItems)
                   .HasForeignKey(bi => bi.MatchId)
                   .OnDelete(DeleteBehavior.Restrict); // Também é bom evitar cascata aqui

            builder.HasOne(bi => bi.Bet)
                   .WithMany(b => b.Items)
                   .HasForeignKey(bi => bi.BetId)
                   .OnDelete(DeleteBehavior.Cascade); // Permitido aqui
        }
    }
}
