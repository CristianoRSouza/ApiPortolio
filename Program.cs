using ApiEntregasMentoria.Configuration;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            // Adiciona variáveis de ambiente às configurações
            builder.Configuration.AddEnvironmentVariables();

            // Configuração de dependências usando Extension Methods
            builder.Services
                .AddSwaggerConfiguration()
                .AddDatabase(builder.Configuration)
                .AddJwtAuthentication(builder.Configuration)
                .AddApplicationServices()
                .AddRepositories()
                .AddValidationServices()
                .AddCorsConfiguration();

            // Configurar porta para Railway
            if (Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") != null)
            {
                builder.WebHost.UseUrls("http://*:8080");
            }
            else
            {
                // Desenvolvimento local
                builder.WebHost.UseUrls("http://+:8080");
            }

            var app = builder.Build();

            // Criar tabelas automaticamente
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MyContext>();
                context.Database.Migrate();
                Console.WriteLine("✅ Migrations executadas");
                
                DatabaseSeeder.SeedData(context);
                Console.WriteLine("✅ Seed data executado");
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiEntregasMentoria v1");
                c.RoutePrefix = "";
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Abrir Swagger no browser automaticamente
            if (app.Environment.IsDevelopment())
            {
                var url = "http://localhost:8080";
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                    Console.WriteLine($"🌐 Swagger aberto automaticamente: {url}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erro ao abrir browser: {ex.Message}");
                    Console.WriteLine($"🔗 Acesse manualmente: {url}");
                }
            }
            app.Run();
        }
    }
}