using System;
using VenturaJobsHR.Application.Records.Jobs.Miscellaneous;
using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Jobs;

public record GetApplicationJobsRecord(
    string Id,
    string Name, 
    string Description,
    SalaryRecord Salary,
    LocationRecord Location,
    JobStatusEnum Status,
    OccupationAreaEnum OccupationArea,
    DateTime DeadLine
);