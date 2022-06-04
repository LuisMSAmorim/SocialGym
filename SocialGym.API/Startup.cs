using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialGym.API.Config;
using SocialGym.BLL.Entities;
using SocialGym.BLL.Interfaces;
using SocialGym.DAL.Context;
using SocialGym.DAL.Repositories;
using System.Text;

namespace SocialGym.API;

public sealed class Startup
{
    public IConfigurationRoot Configuration { get; }
    public Startup(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddDbContext<SocialGymDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

        services.AddIdentity<User, IdentityRole>(options =>
            options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<SocialGymDbContext>();

        var jwtSection = Configuration.GetSection("JwtBearerTokenSettings");

        services.Configure<JwtBearerTokenSettings>(jwtSection);

        var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

        var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtBearerTokenSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtBearerTokenSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        DependencyInjectionServices(services);

        ConfigureSwagger(services);
    }

    private static void DependencyInjectionServices(IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<ICommunitiesRepository, CommunitiesRepository>();
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header (Type 'Bearer' + your token)"
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id  = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}
