using System.Collections.Generic;
using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Applications;

public record ApplicationResponse(UserRecord User, string JobId, List<JobApplicationCriteriaRecord> CriteriaList, double Average);
public record JobApplicationCriteriaRecord(string CriteriaId, ProfileTypeEnum ProfileType);
public record UserRecord(string Name, string Phone, string email, UserTypeEnum UserType);
