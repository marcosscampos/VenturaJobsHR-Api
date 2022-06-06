namespace VenturaJobsHR.CrossCutting.Enums;

public enum EntityError
{
    InvalidSalary,
    SalaryNotZero,
    InvalidDeadLine,
    InvalidJobName,
    InvalidJobDescription,
    DeadLineLessCreationDate,
    DeadLineLessDateNow,
    InvalidLocation,
    CompanyInvalidName,
    JobInvalidLocation,
    JobInvalidSalary,
    JobInvalidCompany,
    DuplicatedItems,
    InvalidJobObject,
    InvalidCriteriaName,
    InvalidCriteriaDescription,
    InvalidCriteriaWeight,
    InvalidCriteria,
    JobDeadLineGreaterThan30Days,
    InvalidOccupationArea,
    JobApplicationUserIdInvalid,
    JobApplicationJobIdInvalid,
    JobApplicationDuplicated,
    JobDuplicated
}
