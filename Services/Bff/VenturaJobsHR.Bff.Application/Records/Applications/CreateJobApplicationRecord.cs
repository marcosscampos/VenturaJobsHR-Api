namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record CreateJobApplicationRecord(string UserId, string JobId, List<JobCriteriaRecord> CriteriaList);