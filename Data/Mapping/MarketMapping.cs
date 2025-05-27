using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class MarketMapping : IEntityTypeConfiguration<Market>
    {
        public void Configure(EntityTypeBuilder<Market> builder)
        {
            builder.ToTable("Markets");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.NameEnum)
                   .IsRequired()
                   .HasConversion<string>();  // Salvar Enum como string

            builder.HasOne(m => m.Match)
                   .WithMany(mt => mt.Markets)
                   .HasForeignKey(m => m.MatchId);
        }
    }
}
