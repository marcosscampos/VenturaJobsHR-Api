using FluentValidation;
using System.Linq.Expressions;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Commands;
using VenturaJobsHR.Domain.SeedWork.Specification;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

public class SearchJobsQuery : BaseListQuery<Pagination<Job>>
{
    public override bool IsValid()
    {
        ValidationResult = new SearchJobsValidator().Validate(this);

        return ValidationResult.IsValid;
    }

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

public class SearchJobsValidator : BaseValidator<SearchJobsQuery>
{
    public SearchJobsValidator()
    {
        RuleFor(x => x.Salary).Must(x => x < 0).WithState(x => AddCommandErrorObject(EntityError.InvalidSalary, x.Salary.ToString()));
    }
}