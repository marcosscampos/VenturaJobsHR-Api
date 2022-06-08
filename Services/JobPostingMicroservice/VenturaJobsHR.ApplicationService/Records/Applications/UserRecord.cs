using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Applications;

public record UserRecord(string Name, string Phone, string email, UserTypeEnum UserType, bool Active);