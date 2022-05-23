using VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;

namespace VenturaJobsHR.Bff.Application.Records.Job;

public record CreateJobRecord(List<JobRequest> JobList);
