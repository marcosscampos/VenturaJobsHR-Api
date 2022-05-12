using MassTransit;
using Microsoft.Extensions.Hosting;

namespace VenturaJobsHR.Consumer;

public class Worker : IHostedService
{
    private readonly IBusControl _bus;

    public Worker(IBusControl bus)
    {
        _bus = bus;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
        => await _bus.StartAsync(cancellationToken);

    public async Task StopAsync(CancellationToken cancellationToken)
        => await _bus.StopAsync(cancellationToken);
}
