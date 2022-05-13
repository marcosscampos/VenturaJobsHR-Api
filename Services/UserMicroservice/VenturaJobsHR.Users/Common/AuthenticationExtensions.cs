using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace VenturaJobsHR.Users.Common;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(AuthenticationOptions builder)
    {
        builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    public static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
    {
        options.Authority = configuration["Firebase:SecureToken"];

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Firebase:SecureToken"],
            ValidateAudience = true,
            ValidAudience = configuration["Firebase:ProjectId"],
            ValidateLifetime = true
        };
    }
}
