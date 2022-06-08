using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Jobs;

public record CancelJobPostingRecord(string Id, JobStatusEnum Status);