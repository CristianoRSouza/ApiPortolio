using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Shared.Enums;
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

            builder.HasData(
               new RolesToken { Id = 1, Role = RoleType.Admin.ToString() },
               new RolesToken { Id = 2, Role = RoleType.Manager.ToString() },
               new RolesToken { Id = 3, Role = RoleType.Client.ToString() }
           );
        }
    }
}
