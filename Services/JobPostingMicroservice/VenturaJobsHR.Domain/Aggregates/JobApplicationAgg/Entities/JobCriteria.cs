using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;

public class JobCriteria
{
    public JobCriteria(string criteriaId, ProfileTypeEnum answer)
    {
        CriteriaId = criteriaId;
        Answer = answer;
    }

    public JobCriteria()
    {

    }

    public string CriteriaId { get; set; }
    public ProfileTypeEnum Answer { get; set; }
}