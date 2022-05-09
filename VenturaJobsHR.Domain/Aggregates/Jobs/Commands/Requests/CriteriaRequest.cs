using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class CriteriaRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProfileTypeEnum Profiletype { get; set; }
    public int Weight { get; set; }
}
