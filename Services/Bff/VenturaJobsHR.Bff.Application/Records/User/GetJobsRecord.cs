using VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;
using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.User;

public record GetJobsRecord(
    string Id,
    string Name, 
    string Description,
    SalaryRequest Salary,
    LocationRequest Location,
    CompanyRequest Company,
    FormOfHiringEnum FormOfHiring,
    List<CriteriaRequest> CriteriaList,
    JobStatusEnum Status,
    OccupationAreaEnum OccupationArea,
    DateTime DeadLine,
    double ProfileAverage,
    bool Active);