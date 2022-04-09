using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.Domain.SeedWork.Entities;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

public class Job : Entity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Salary Salary { get; set; }
    public DateTime FinalDate { get; set; }
    public DateTime CreationDate { get; set; }

    public Job(string name, string description, Salary salary, DateTime finalDate)
    {
        Name = name;
        Description = description;
        Salary = salary;
        FinalDate = finalDate;
        CreationDate = new DateTimeWithZone(DateTime.Now).LocalTime;
    }

    public Job(string id, string name, string description, Salary salary, DateTime finalDate) : this(name, description, salary, finalDate)
    {
        Id = id;
    }
}
