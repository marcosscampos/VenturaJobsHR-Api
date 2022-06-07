namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;

public class JobApplicationRequest
{
    public string UserId { get; set; }
    public string JobId { get; set; }
    public List<JobCriteriaRequest> CriteriaList { get; set; }
    public string GetReference()
        => $"JOBAPPLICATION-#{UserId.ToUpper()}#{JobId.ToUpper()}";
}
