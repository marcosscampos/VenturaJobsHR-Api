namespace VenturaJobsHR.Domain.Aggregates.Common.Entities;

public abstract class Entity
{
    public DateTime CreationDate { get; set; }
    public DateTime? LastUpdate { get; set; }
}
