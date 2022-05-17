namespace VenturaJobsHR.CrossCutting.Enums;

public enum EntityError
{
    InvalidSalary,
    SalaryNotZero,
    InvalidFinalDate,
    InvalidJobName,
    InvalidJobDescription,
    FinalDateLessCreationDate,
    FinalDateLessDateNow,
    InvalidLocation,
    CompanyInvalidName,
    JobInvalidLocation,
    JobInvalidSalary,
    JobInvalidCompany,
    DuplicatedItems,
    InvalidJobObject
}
