namespace VenturaJobsHR.Domain.Aggregates.Jobs.Entities
{
    public class Salary
    {
        public decimal Value { get; set; }

        public Salary(decimal salary) => Value = salary;
    }
}