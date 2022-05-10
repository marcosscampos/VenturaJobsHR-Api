using System.Linq.Expressions;

namespace VenturaJobsHR.Domain.SeedWork.Specification
{
    internal class AndSpecification<T> : CompositeSpecification<T> where T : class
    {

        private ISpecification<T> _rightSpecification;
        private ISpecification<T> _leftSpecification;

        public AndSpecification(ISpecification<T> rightSpecification, ISpecification<T> leftSpecification)
        {
            _rightSpecification = rightSpecification;
            _leftSpecification = leftSpecification;
        }

        public override ISpecification<T> RightSpecification => _rightSpecification;
        public override ISpecification<T> LeftSpecification => _leftSpecification;

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            Expression<Func<T, bool>> right = _rightSpecification.IsSatisfiedBy();
            Expression<Func<T, bool>> left = _leftSpecification.IsSatisfiedBy();

            return left.And(right);
        }
    }
}