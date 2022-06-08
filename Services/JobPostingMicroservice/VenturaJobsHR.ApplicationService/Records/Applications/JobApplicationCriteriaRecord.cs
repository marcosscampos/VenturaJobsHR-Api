using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Applications;

public record JobApplicationCriteriaRecord(string CriteriaId, ProfileTypeEnum ProfileType);