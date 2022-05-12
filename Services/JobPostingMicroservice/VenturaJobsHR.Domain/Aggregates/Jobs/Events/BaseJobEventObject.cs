using VenturaJobsHR.Domain.SeedWork.Events;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Events;

public class BaseJobEventObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Salary { get; set; }
    public DateTime FinalDate { get; set; }
}

public class JobsCreatedEvent : BaseNotification<BaseJobEventObject>
{
}

public class JobsConsumedEvent : BaseNotification<BaseJobEventObject>
{
    public TimeSpan TimeElapsed => DateTime.Now - this.CreatedAt;
}

public class JobsUpdatedEvent : BaseNotification<BaseJobEventObject>
{
    public DateTime LastModificationDate { get; set; }
}