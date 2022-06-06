namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities
{
    public class Salary
    {
        public decimal Value { get; set; }

        public Salary(decimal salary) => Value = salary;
        public Salary() { }
    }
}