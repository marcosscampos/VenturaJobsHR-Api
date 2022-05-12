using VenturaJobsHR.Message.Enums;

namespace VenturaJobsHR.Message.Dto.Job;

public class CriteriaDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ProfileTypeEnum Profiletype { get; set; }
    public int Weight { get; set; }
}
