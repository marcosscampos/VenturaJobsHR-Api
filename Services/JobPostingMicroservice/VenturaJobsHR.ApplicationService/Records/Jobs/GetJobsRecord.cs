using System;
using System.Collections.Generic;
using VenturaJobsHR.Application.Records.Jobs.Miscellaneous;
using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Jobs;

public record GetJobsRecord(
    string Id,
    string Name, 
    string Description,
    SalaryRecord Salary,
    LocationRecord Location,
    CompanyRecord Company,
    FormOfHiringEnum FormOfHiring,
    List<CriteriaRecord> CriteriaList,
    JobStatusEnum Status,
    OccupationAreaEnum OccupationArea,
    DateTime DeadLine,
    double ProfileAverage,
    bool Active);