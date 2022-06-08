using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using System.IO.Compression;
using System.Reflection;
using VenturaJobsHR.Api.Common.Extensions;
using VenturaJobsHR.Api.Common.Middlewares;
using VenturaJobsHR.Api.Seedwork.Swagger;
using VenturaJobsHR.Common.Extensions;
using Coravel;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using VenturaJobsHR.Api.Common.DI;
using VenturaJobsHR.Api.Common.Jobs;

var builder = WebApplication.CreateBuilder(args);

var host = new HostBuilder();
host.ConfigureAppConfiguration((HostBuilderContext context, IConfigurationBuilder _builder) =>
{
    _builder.AddEnvironmentVariables().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
}).ConfigureLogging((hostingContext, logging) =>
{
    var logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose,
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
            theme: SystemConsoleTheme.Literate).CreateLogger();

    logging.ClearProviders();
    logging.AddSerilog(logger);
});
// Add services to the container.
builder.Services.ConfigureApplicationDependencies(builder.Configuration);


builder.Services.AddControllers().AddNewtonsoftJson(x =>
{
    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    x.SerializerSettings.Formatting = Formatting.Indented;
    x.SerializerSettings.ContractResolver = new DefaultContractResolver();
    x.UseCamelCasing(true);
});
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.Configure<Email>(email =>
{
    email.Host = builder.Configuration["Host"];
    email.Login = builder.Configuration["Login"];
    email.Port = int.Parse(builder.Configuration["Port"]);
    email.Password = builder.Configuration["Password"];
    email.SSL = bool.Parse(builder.Configuration["SSL"]);
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

builder.Services.AddSwaggerGen(x =>
{
    SwaggerConfiguration.Configure(x, "v1");
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddAuthentication(x =>
{
    AuthenticationExtensions.ConfigureAuthentication(x);
}).AddJwtBearer(x =>
{
    AuthenticationExtensions.ConfigureJwtBearer(x, builder.Configuration);
});

var app = builder.Build();
// app.Services.UseScheduler(scheduler =>
// {
//     scheduler.Schedule<Worker>().DailyAt(00, 30);
// });

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseCors(CORS_DEFAULT_POLICY);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.Run();

public partial class Program { }