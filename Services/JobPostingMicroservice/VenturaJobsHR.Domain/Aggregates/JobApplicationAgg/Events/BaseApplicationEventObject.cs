using VenturaJobsHR.Domain.Aggregates.Common.Events;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Events.Miscellaneous;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Events;

public class BaseJobApplicationEventObject
{
    public string UserId { get; set; }
    public string JobId { get; set; }
    public List<JobCriteriaEvent> CriteriaList { get; set; }
}

public class JobApplicationCreatedEvent : BaseNotification<BaseJobApplicationEventObject>
{

}
