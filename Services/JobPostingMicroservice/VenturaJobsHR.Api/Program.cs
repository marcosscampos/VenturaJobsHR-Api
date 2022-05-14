using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO.Compression;
using VenturaJobsHR.Api.Common;
using VenturaJobsHR.Api.Common.ErrorsHandler;
using VenturaJobsHR.Api.Docs;
using VenturaJobsHR.Application.DI;
using VenturaJobsHR.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

var host = new HostBuilder();
host.ConfigureAppConfiguration((HostBuilderContext context, IConfigurationBuilder _builder) =>
{
    _builder.AddEnvironmentVariables().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
});
// Add services to the container.
builder.Services.ConfigureApplicationDependencies(builder.Configuration);

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

const string CORS_DEFAULT_POLICY = "_corsConfig";
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
builder.Services.AddOpenApiDocument(x =>
{
    OpenApiConfiguration.Configure(x, "v1");
});

builder.Services.AddAuthentication(x =>
{
    AuthenticationExtensions.ConfigureAuthentication(x);
}).AddJwtBearer(x =>
{
    AuthenticationExtensions.ConfigureJwtBearer(x, builder.Configuration);
});

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    ctx.Response.OnStarting(() =>
    {
        ctx.Response.Headers[HeaderNames.Date] = new string[] { DateTimeExtensions.ConvertDateTime(DateTime.UtcNow).ToString() };

        return Task.CompletedTask;
    });

    await next();
});

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseDeveloperExceptionPage();

app.UseCors(CORS_DEFAULT_POLICY);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }