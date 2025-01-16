using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class RolesTokenMaping : IEntityTypeConfiguration<RolesToken>
    {
        public void Configure(EntityTypeBuilder<RolesToken> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Role).IsRequired();
        }
    }
}
