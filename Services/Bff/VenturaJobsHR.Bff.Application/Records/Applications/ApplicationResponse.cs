namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record ApplicationResponse(UserRecord User, string JobId, List<JobApplicationCriteriaRecord> CriteriaList, double Average);