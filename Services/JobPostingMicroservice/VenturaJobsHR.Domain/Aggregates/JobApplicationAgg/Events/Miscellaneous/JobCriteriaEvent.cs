using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Events.Miscellaneous;

public class JobCriteriaEvent
{
    public string CriteriaId { get; set; }
    public ProfileTypeEnum ProfileType { get; set; }
}
