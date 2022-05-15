using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.Common.Events;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events.Miscellaneous;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Events;

public class BaseJobEventObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SalaryEvent Salary { get; set; }
    public LocationEvent Location { get; set; }
    public CompanyEvent Company { get; set; }
    public IList<CriteriaEvent> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }
    public bool Active { get; set; }
}

public class JobsCreatedEvent : BaseNotification<BaseJobEventObject>
{
}

public class JobsUpdatedEvent : BaseNotification<BaseJobEventObject>
{
    public DateTime LastModificationDate { get; set; }
}