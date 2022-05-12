using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace VenturaJobsHR.Consumer;

public class Program
{
    public static void Main(string[] args)
        => CreateWebHostBuilder(args).Build().Run();

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        => WebHost.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(app =>
        {
            app.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
        })
        .UseStartup<Startup>();
}