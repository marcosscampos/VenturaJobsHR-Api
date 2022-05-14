using MediatR;

namespace VenturaJobsHR.Domain.Aggregates.Common.Events;

public class BaseNotification<T> : INotification where T : class
{

    public BaseNotification()
    {
        CreatedAt = DateTime.Now;
    }

    public T Event { get; set; }
    public DateTime CreatedAt { get; set; }
}
