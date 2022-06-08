using VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;
using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.Job;

public record GetApplicationJobsRecord(
    string Id,
    string Name, 
    string Description,
    SalaryRequest Salary,
    LocationRequest Location,
    JobStatusEnum Status,
    DateTime DeadLine
);