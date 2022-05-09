using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

public class Criteria
{

    public Criteria(string name, string description, ProfileTypeEnum profileType, int weight)
    {
        Name = name;
        Description = description;
        Profiletype = profileType;
        Weight = weight;
    }

    public Criteria()
    {

    }

    public string Name { get; set; }
    public string Description { get; set; }
    public ProfileTypeEnum Profiletype { get; set; }
    public int Weight { get; set; }
}
