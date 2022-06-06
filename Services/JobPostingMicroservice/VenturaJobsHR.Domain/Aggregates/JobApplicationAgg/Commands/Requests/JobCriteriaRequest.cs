using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;

public class JobCriteriaRequest
{
    public string CriteriaId { get; set; }
    public ProfileTypeEnum ProfileType { get; set; }
}
