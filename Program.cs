
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Repositories;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.JwtConfig;
using ApiEntregasMentoria.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Text;

namespace ApiEntregasMentoria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var jwtSecret = builder.Configuration["JWT_SECRET"];

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Adiciona variáveis de ambiente às configurações
            builder.Configuration.AddEnvironmentVariables();
            builder.Services.AddDbContext<MyContext>(options => options.UseNpgsql(
            builder.Configuration.GetConnectionString("MyConnectionPostGres"),
            npgsqlOptions => { npgsqlOptions.CommandTimeout(120); }));

            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRepositoryUser, UserRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //builder.Services.AddDbContext<MyContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"), sqlOptions => sqlOptions.CommandTimeout(120)
            //));
            var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT_SECRET"]);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            builder.Services.Configure<JwtSettings>(options =>
            {
                options.Secret = builder.Configuration["JWT_SECRET"];
            });




            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
