namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record ApplicationResponse(
    UserRecord User, 
    string JobId, 
    DateTime CreatedAt, 
    List<JobApplicationCriteriaRecord> CriteriaList, 
    double Average);