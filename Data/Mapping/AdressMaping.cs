using ApiEntregasMentoria.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class AdressMaping : IEntityTypeConfiguration<Adress>
    {
        public void Configure(EntityTypeBuilder<Adress> builder)
        {
            builder.Property(a => a.City)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Neighborhood)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Street)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Phone)
                .HasMaxLength(15)
                .IsRequired();

        }
    }
}
