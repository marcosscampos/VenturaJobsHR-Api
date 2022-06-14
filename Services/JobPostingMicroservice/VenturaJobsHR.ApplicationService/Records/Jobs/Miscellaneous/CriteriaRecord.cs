using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Application.Records.Jobs.Miscellaneous;

public record CriteriaRecord(
    string? Id,
    string Name,
    string Description,
    ProfileTypeEnum ProfileType,
    int Weight);
