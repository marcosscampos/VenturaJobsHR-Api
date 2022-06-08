using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.Applications;

public record JobApplicationCriteriaRecord(string CriteriaId, ProfileTypeEnum ProfileType);