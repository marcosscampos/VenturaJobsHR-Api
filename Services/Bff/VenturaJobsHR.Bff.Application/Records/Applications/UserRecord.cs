using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record UserRecord(string Name, string Phone, string email, UserTypeEnum UserType);