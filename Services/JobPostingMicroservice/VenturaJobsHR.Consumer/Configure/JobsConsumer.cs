using MassTransit;
using Microsoft.Extensions.Configuration;
using VenturaJobsHR.Consumer.Consumers;

namespace VenturaJobsHR.Consumer.Configure;

public class JobsConsumer
{
    public static void AddRabbitMq(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
    {
        cfg.ReceiveEndpoint(configuration["MessagesConfiguration:Queues:Jobs"], ep =>
        {
            ep.PrefetchCount = 10;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<JobConsumer>(context);
        });
    }
}
