using System.Linq.Expressions;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Commands;
using VenturaJobsHR.Domain.SeedWork.Specification;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

public class SeachJobsQuery
{
    public decimal Salary { get; set; }
    public DateTime FinalDate { get; set; }

    public Expression<Func<Job, bool>> BuildFilter()
    {
        Specification<Job> filter = new DirectSpecification<Job>(x => x.CreationDate > DateTime.MinValue);

        if (Salary != 0)
            filter &= new DirectSpecification<Job>(x => x.Salary.Value >= Salary);

        if (FinalDate > DateTime.MinValue)
            filter &= new DirectSpecification<Job>(x => x.FinalDate >= FinalDate);

        return filter.IsSatisfiedBy();
    }
}
