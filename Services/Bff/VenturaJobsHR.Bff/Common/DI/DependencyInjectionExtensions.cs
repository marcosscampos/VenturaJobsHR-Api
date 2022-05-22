using VenturaJobsHR.Bff.CrossCutting.Settings.Interfaces;
using VenturaJobsHR.Bff.CrossCutting.Settings.Concrete;
using VenturaJobsHR.Bff.CrossCutting.Enums;
using System.Net;

namespace VenturaJobsHR.Bff.Common.DI;

public static class DependencyInjectionExtensions
{
    public static void ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(serviceSettings =>
        {
            serviceSettings.UrlApiJobsV1 = configuration["APIs:Jobs"];
            serviceSettings.UrlApiUsersV1 = configuration["APIs:Users"];
            serviceSettings.Timeout = int.Parse(configuration["APIs:Timeout"]);
        });

        services.AddHttpClients(configuration);
    }

    public static void AddServices(this IServiceCollection service, Action<IServiceSettings> serviceSettings)
    {
        IServiceSettings configureServices = new ServiceSettings();
        serviceSettings.Invoke(configureServices);
        service.AddSingleton(configureServices);
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var listOfClients = new Dictionary<HttpClientKeysEnum, string>
        {
            { HttpClientKeysEnum.Jobs, configuration["APIs:Jobs"] },
            { HttpClientKeysEnum.Users, configuration["APIs:Users"] }
        };

        var listOfTimeout = new Dictionary<HttpClientKeysEnum, double>
            {
                { HttpClientKeysEnum.Jobs, double.Parse(configuration["APIs:Timeout"]) },
                { HttpClientKeysEnum.Users, double.Parse(configuration["APIs:Timeout"]) },
            };


        foreach(var (key, value) in listOfClients)
        {
            services.AddHttpClient(key.ToString(), client =>
            {
                client.BaseAddress = new Uri(value);
                client.DefaultRequestHeaders.Add("Accept-Encoding", "br, gzip, deflate");

                if (listOfTimeout.ContainsKey(key))
                    client.Timeout = TimeSpan.FromMilliseconds(listOfTimeout.GetValueOrDefault(key));
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            }).AddHeaderPropagation();
        }
    }
}
