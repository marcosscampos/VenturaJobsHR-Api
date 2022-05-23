using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;

public class CriteriaRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProfileTypeEnum Profiletype { get; set; }
    public int Weight { get; set; }
}