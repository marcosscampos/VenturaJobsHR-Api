using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Authentication;
using VenturaJobsHR.Consumer.Configure;
using VenturaJobsHR.Consumer.Consumers;

namespace VenturaJobsHR.Consumer.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureConsummerApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseMassTransit(configuration);

        return services;
    }

    private static void UseMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var username = configuration["MessagesConfiguration:Username"];
        var password = configuration["MessagesConfiguration:Password"];
        var url = configuration["MessagesConfiguration:AmqpUrl"];

        services.AddMassTransit(x =>
        {
            AddConsumers(x);

            x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(url), h =>
                {
                    h.Username(configuration["MessagesConfiguration:Username"]);
                    h.Password(configuration["MessagesConfiguration:Password"]);

                    h.UseSsl(s =>
                    {
                        s.Protocol = SslProtocols.Tls12;
                    });
                });

                JobsConsumer.AddRabbitMq(context, cfg, configuration);
            }));
        });

    }

    private static void AddConsumers(IRegistrationConfigurator service)
    {
        service.AddConsumer<JobConsumer>();
    }
}
