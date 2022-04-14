using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace VenturaJobsHR.Api.Common;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(AuthenticationOptions builder)
    {
        builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    public static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
    {
        options.Authority = $"{configuration["Keycloak:Url"]}/realms/{configuration["Keycloak:Realm"]}";
        options.Audience = configuration["Keycloak:ClientId"];
        options.RequireHttpsMetadata = bool.Parse($"{configuration["Keycloak:RequireHttps"]}");
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            NameClaimType = "user_login"
        };
    }
}
