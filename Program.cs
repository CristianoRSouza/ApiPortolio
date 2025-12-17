using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.Repositories;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.JwtConfig;
using ApiEntregasMentoria.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ApiEntregasMentoria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Adiciona suporte ao JWT no Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiEntregasMentoria", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Digite: Bearer {seu token JWT}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Adiciona vari�veis de ambiente �s configura��es
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<PixService>();
            builder.Services.AddScoped<BetService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<ProfileService>();
            builder.Services.AddScoped<IRepositoryUser, UserRepository>();
            builder.Services.AddScoped<IRepositoryTeam, TeamRepository>();

            builder.Services.AddScoped<IRepositoryMatch, MatchRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddDbContext<MyContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                                     builder.Configuration.GetConnectionString("DefaultConnection") ??
                                     "Host=localhost;Database=soccerbet;Username=postgres;Password=123456;Port=5432;Timeout=30;CommandTimeout=30;Pooling=true;MinPoolSize=1;MaxPoolSize=10;ConnectionIdleLifetime=300;KeepAlive=30";
                
                Console.WriteLine($"🔍 Using connection: {connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0))}...");
                options.UseNpgsql(connectionString);
            });

            // configura jwt
            builder.Services.AddScoped<TokenService>();
            var jwtSecret = builder.Configuration["Jwt:Key"] ?? "SoccerBetSecretKey123456789012345678901234567890123456789012345678901234567890";
            var key = Encoding.ASCII.GetBytes(jwtSecret);
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
                options.Secret = jwtSecret;
            });

            // CORS para React Native
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactNative", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            //configura jwt com valores do app settings
            //var jwtSettingsConfig = builder.Configuration.GetSection("JwtSettings");
            //builder.Services.Configure<JwtSettings>(jwtSettingsConfig);
            //var jwtSettings = jwtSettingsConfig.Get<JwtSettings>();
            //var keyConfigJwtAppSetting = Encoding.ASCII.GetBytes(jwtSettings!.Secret);

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = true;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = jwtSettings.Audience,
            //        ValidIssuer = jwtSettings.Sender
            //    };
            //});


// Configurar porta para Railway
// Configurar porta
if (Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") != null)
{
    builder.WebHost.UseUrls("http://*:8080");
}
else
{
    // Desenvolvimento local
    builder.WebHost.UseUrls("https://localhost:7207", "http://localhost:5000");
}

            var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors("AllowReactNative");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
