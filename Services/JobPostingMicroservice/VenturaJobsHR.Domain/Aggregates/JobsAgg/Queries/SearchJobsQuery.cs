using FluentValidation;
using MediatR;
using System.Linq.Expressions;
using Newtonsoft.Json;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.SeedWork.Specification;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;

public class SearchJobsQuery : BaseListQuery<Job>, IRequest<Pagination<Job>>
{
    public override bool IsValid()
    {
        ValidationResult = new SearchJobsValidator().Validate(this);

        return ValidationResult.IsValid;
    }

    public decimal Salary { get; set; }
    public DateTime DeadLine { get; set; }
    public int OccupationArea { get; set; }
    
    [JsonIgnore]
    public string? Uid { get; set; }

    public Expression<Func<Job, bool>> BuildFilter()
    {
        Specification<Job> filter = new DirectSpecification<Job>(x => x.CreationDate > DateTime.MinValue);

        if (Salary != 0)
            filter &= new DirectSpecification<Job>(x => x.Salary.Value >= Salary);

        if (DeadLine > DateTime.MinValue)
            filter &= new DirectSpecification<Job>(x => x.DeadLine >= DeadLine);

        if (OccupationArea > 0)
            filter &= new DirectSpecification<Job>(x => x.OccupationArea == Job.GetOccupationAreaBy(OccupationArea));

        if (Uid is not null)
            filter &= new DirectSpecification<Job>(x => x.Company.Uid == Uid);

        return filter.IsSatisfiedBy();
    }
}

public class SearchJobsValidator : BaseValidator<SearchJobsQuery>
{
    public SearchJobsValidator()
    {
        RuleFor(x => x.Salary).Must(x => x > 0).WithState(x => AddCommandErrorObject(EntityError.InvalidSalary, x.Salary.ToString()));
        RuleFor(x => x.OccupationArea).Must(x => x >= 5).WithState(x => AddCommandErrorObject(EntityError.InvalidOccupationArea, x.OccupationArea.ToString()));
    }
}