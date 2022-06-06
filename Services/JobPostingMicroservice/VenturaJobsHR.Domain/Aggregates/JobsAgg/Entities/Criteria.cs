using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

public class Criteria
{

    public Criteria(string id, string name, string description, ProfileTypeEnum profileType, int weight)
    {
        Id = id;
        Name = name;
        Description = description;
        Profiletype = profileType;
        Weight = weight;
    }

    public Criteria()
    {

    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProfileTypeEnum Profiletype { get; set; }
    public int Weight { get; set; }
}
