using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;

public class JobCriteria
{
    public JobCriteria(string criteriaId, ProfileTypeEnum profileType)
    {
        CriteriaId = criteriaId;
        ProfileType = profileType;
    }

    public JobCriteria()
    {

    }

    public string CriteriaId { get; set; }
    public ProfileTypeEnum ProfileType { get; set; }
}