using MassTransit;
using VenturaJobsHR.Message.Interface;

namespace VenturaJobsHR.Message.Service;

public class BusService : IBusService
{
    private readonly IBus _bus;
    public BusService(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessageToQueueAsync(object obj, string queue)
    {
        Uri uri = new($"queue:{queue}");
        var endpoint = await _bus.GetSendEndpoint(uri);

        await endpoint.Send(obj);
    }
}
