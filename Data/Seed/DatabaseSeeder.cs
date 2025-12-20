using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static void SeedData(MyContext context)
        {
            SeedRoles(context);
        }

        private static void SeedRoles(MyContext context)
        {
            if (context.Roles.Any()) return;

            var roles = new[]
            {
                new Role { Id = 1, Name = "User", Description = "Regular user with basic permissions" },
                new Role { Id = 2, Name = "Admin", Description = "Administrator with elevated permissions" },
                new Role { Id = 3, Name = "Moderator", Description = "Moderator with content management permissions" },
                new Role { Id = 4, Name = "SuperAdmin", Description = "Super administrator with full system access" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
