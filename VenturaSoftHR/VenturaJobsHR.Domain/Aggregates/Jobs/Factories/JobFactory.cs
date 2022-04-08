using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Factories;

public static class JobFactory
{
    public static Job Create(string name, string description, decimal salary, DateTime finalDate)
       => new(name, description, new Salary(salary), finalDate);

    public static Job Create(string id, string name, string description, decimal salary, DateTime finalDate)
        => new(id, name, description, new Salary(salary), finalDate);
}
