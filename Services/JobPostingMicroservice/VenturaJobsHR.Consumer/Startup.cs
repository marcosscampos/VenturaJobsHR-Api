using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VenturaJobsHR.Consumer.Common;
using VenturaJobsHR.Application.DI;

namespace VenturaJobsHR.Consumer;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<Worker>();
        services.ConfigureConsummerApplicationDependencies(Configuration);
        services.ConfigureApplicationDependencies(Configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
    }
}
