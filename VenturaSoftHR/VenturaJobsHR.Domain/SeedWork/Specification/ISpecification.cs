using System.Linq.Expressions;

namespace VenturaJobsHR.Domain.SeedWork.Specification
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> IsSatisfiedBy();
    }
}