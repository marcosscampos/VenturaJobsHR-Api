using Microsoft.AspNetCore.Http.Json;
using VenturaJobsHR.Users.Application.DI;

namespace VenturaJobsHR.Users.Common;

public static class ServiceCollectionExtensionsWebApplication
{
    const string CORS_DEFAULT_POLICY = "_corsConfig";

    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CORS_DEFAULT_POLICY, config =>
            {
                config
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(
                    "http://localhost:3000",
                    "https://localhost:3000",
                    "https://ventura-jobs-hr.netlify.app");

            });
        });

        builder.Services.Configure<JsonOptions>(o =>
        {
            o.SerializerOptions.WriteIndented = true;
        });

        builder.Services.ConfigureApplicationDependencies(builder.Configuration);

        return builder;
    }

    public static WebApplication UseConfiguration(this WebApplication app)
    {
        app.UseCors(CORS_DEFAULT_POLICY);
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapCarter();

        return app;
    }
}
