using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiEntregasMentoria.Data.Mapping
{
    public class UserMaping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //para uso de banco SQLSERVER
            //builder.HasKey(a => a.Id);
            //builder.Property(a => a.Name).IsRequired();
            //builder.Property(a => a.Surname).IsRequired();
            //builder.Property(a => a.Email).IsRequired();
            //builder.Property(a => a.Password).IsRequired();
            //builder.Property(a => a.CreateAt).HasDefaultValueSql("GETUTCDATE()").IsRequired();

            //para uso de banco POSTGRES
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.Surname).IsRequired();
            builder.Property(a => a.Email).IsRequired();
            builder.Property(a => a.Password).IsRequired();
            builder.Property(a => a.RoleId);
            builder.Property(a => a.AdressId);

            builder.Property(a => a.RoleId)
              .IsRequired(false);

            builder.Property(a => a.AdressId)
                   .IsRequired(false);

            builder.Property(a => a.CreateAt)
                .HasDefaultValueSql("NOW()") 
                .IsRequired();

            builder.HasOne(a => a.RolesToken)                 
                   .WithMany(b => b.Users)              
                   .HasForeignKey(a => a.RoleId).IsRequired(false);

            builder.HasOne(a => a.Adress)
                  .WithMany(b => b.Users)
                  .HasForeignKey(a => a.AdressId).IsRequired(false);
        }
    }
}
