namespace VenturaJobsHR.Message.Interface;

public interface IBusService
{
    Task SendMessageToQueueAsync(object obj, string queue);
}
