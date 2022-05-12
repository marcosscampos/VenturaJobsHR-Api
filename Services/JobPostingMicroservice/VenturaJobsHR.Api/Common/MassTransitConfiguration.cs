using MassTransit;
using System.Security.Authentication;

namespace VenturaJobsHR.Api.Common;

public static class MassTransitConfiguration
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var username = configuration["MessagesConfiguration:Username"];
        var password = configuration["MessagesConfiguration:Password"];
        var url = configuration["MessagesConfiguration:AmqpUrl"];

        services.AddMassTransit(x =>
        {
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(new Uri(url), settings =>
                {
                    settings.Username(username);
                    settings.Password(password);

                    settings.UseSsl(s =>
                    {
                        s.Protocol = SslProtocols.Tls12;
                    });
                });
            }));
        });

        return services;
    }
}
