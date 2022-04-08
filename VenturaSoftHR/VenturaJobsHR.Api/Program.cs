using Newtonsoft.Json;
using VenturaJobsHR.Api.Common;
using VenturaJobsHR.Api.Docs.Filters;

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
    x.UseMemberCasing();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.OrderActionsBy((s) => $"{s.ActionDescriptor.RouteValues["controller"]}_{s.HttpMethod}");
    x.DocumentFilter<OperationsOrderingFilter>();
    x.OperationFilter<SwaggerExcludeFilter>();
    x.OperationFilter<JsonIgnoreQueryOperationFilter>();
    x.SchemaFilter<SwaggerIgnoreFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }