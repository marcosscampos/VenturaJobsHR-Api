namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record ApplicationRecord(string UserId, string JobId, List<JobCriteriaRecord> CriteriaList);