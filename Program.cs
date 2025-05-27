using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.Repositories;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.JwtConfig;
using ApiEntregasMentoria.Services;
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

            // Adiciona variáveis de ambiente às configurações
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMatchService, MatchService>();
            builder.Services.AddScoped<IRepositoryUser, UserRepository>();
            builder.Services.AddScoped<IRepositoryTeam, TeamRepository>();
            builder.Services.AddScoped<IRepositoryRoleToken, RoleTokenRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepositoryMatch, MatchRepository>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddDbContext<MyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"), sqlOptions => sqlOptions.CommandTimeout(120)));

            // configura jwt com variavel de ambiente
            builder.Services.AddScoped<TokenService>();
            var jwtSecret = builder.Configuration["JWT_SECRET"];
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
