using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using System.IO.Compression;
using System.Reflection;
using VenturaJobsHR.Bff.Common.DI;
using VenturaJobsHR.Bff.Common.Middlewares;
using VenturaJobsHR.Bff.Seedwork.Swagger;

namespace VenturaJobsHR.Bff.Common;

public static class ServiceCollectionExtensionsWebApplication
{
    const string CORS_DEFAULT_POLICY = "_corsConfig";

    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

        builder.Services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = long.MaxValue;
        });

        builder.Services.AddResponseCompression(opt =>
        {
            opt.EnableForHttps = true;
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(opt =>
        {
            opt.Level = CompressionLevel.Fastest;
        });

        builder.Services.Configure<ForwardedHeadersOptions>(opt =>
        {
            opt.ForwardedHeaders = ForwardedHeaders.All;
            opt.ForwardLimit = null;
        });


        builder.Services.AddApiVersioning(p =>
        {
            p.DefaultApiVersion = new ApiVersion(1, 0);
            p.ReportApiVersions = true;
            p.AssumeDefaultVersionWhenUnspecified = true;
        });

        builder.Services.AddVersionedApiExplorer(p =>
        {
            p.GroupNameFormat = "'v'VVV";
            p.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(x =>
        {
            SwaggerConfiguration.Configure(x, "v1");
        });
        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHeaderPropagation(configure =>
         {
             configure.Headers.Add("Authorization");
         });

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

        builder.Services.ConfigureApplicationDependencies(builder.Configuration);

        builder.Services.AddAuthentication(x =>
        {
            AuthenticationExtensions.ConfigureAuthentication(x);
        }).AddJwtBearer(x =>
        {
            AuthenticationExtensions.ConfigureJwtBearer(x, builder.Configuration);
        });

        return builder;
    }

    public static WebApplication UseConfiguration(this WebApplication app)
    {
        app.UseMiddleware<ApiExceptionMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseDeveloperExceptionPage();

        app.UseCors(CORS_DEFAULT_POLICY);

        app.UseHttpsRedirection();
        app.UseHeaderPropagation();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });

        return app;
    }
}
