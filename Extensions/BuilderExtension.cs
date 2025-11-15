using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using passwords_backend.Data;

namespace passwords_backend.Extensions;

public static class BuilderExtension
{

    public static void AddDataBase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionsString");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var key = builder.Configuration["PrivateKey"] ?? "";

        builder.Services
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        });
        builder.Services.AddAuthorization();
    }
    public static void AddSwaggerDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de gerenciamento de senhas", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authentication",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header usando o esquema Bearer."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
             }
});

        });
    }

}