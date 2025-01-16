using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.ContextEntity
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Adress> Adress { get; set; }
        public DbSet<RolesToken> RolesToken { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyContext).Assembly);
            base.OnModelCreating(modelBuilder);

        }


    }
}
