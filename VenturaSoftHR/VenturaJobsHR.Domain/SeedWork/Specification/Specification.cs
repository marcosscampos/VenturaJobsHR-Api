using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace VenturaJobsHR.Domain.SeedWork.Specification;

public abstract class Specification<T> : ISpecification<T> where T : class
{
    public abstract Expression<Func<T, bool>> IsSatisfiedBy();

    public static Specification<T> operator &(Specification<T> right, Specification<T> left)
        => new AndSpecification<T>(right, left);
}

