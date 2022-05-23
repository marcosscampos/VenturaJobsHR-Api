using VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;

namespace VenturaJobsHR.Bff.Application.Records.Job;

public record UpdateJobRecord(List<JobRequest> JobList);
