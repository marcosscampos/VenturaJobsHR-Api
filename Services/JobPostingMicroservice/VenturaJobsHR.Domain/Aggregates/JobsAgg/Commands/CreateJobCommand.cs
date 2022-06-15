using FluentValidation;
using MediatR;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;

public class CreateJobCommand : BaseJobCommand, IRequest<Unit>
{
    public override bool IsValid()
    {
        ValidationResult = new CreateJobValidator().Validate(this);

        return ValidationResult.IsValid;
    }
}

public class CreateJobValidator : BaseValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.JobList).Cascade(CascadeMode.Stop).Must(x => x.Count > 0).WithState(x => AddCommandErrorObject(EntityError.InvalidJobObject));

        RuleFor(x => x.JobList.Select(p => new { p.Name, p.Description, p.DeadLine }).ToList())
            .Cascade(CascadeMode.Stop)
            .Must(list => !list.GroupBy(x => x).Any(y => y.Count() > 1)).WithState(x => AddCommandErrorObject(EntityError.DuplicatedItems));

        RuleForEach(x => x.JobList).ChildRules(job =>
        {
            job.RuleFor(x => x.Salary).SetValidator(x => new SalaryValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Salary).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidSalary, x.GetReference()));
            });

            job.RuleFor(x => x.Location).SetValidator(x => new LocationValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Location).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidLocation, x.GetReference()));
            });

            job.RuleFor(x => x.Company).SetValidator(x => new CompanyValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Company).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidCompany, x.GetReference()));
            });

            job.RuleFor(x => x.CriteriaList).NotNull().NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidCriteria, x.GetReference()));
            job.RuleForEach(x => x.CriteriaList).ChildRules(criteria =>
            {
                criteria.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidCriteriaName));
                criteria.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidCriteriaDescription));
                criteria.RuleFor(x => x.Weight).LessThan(6).WithState(x => AddCommandErrorObject(EntityError.InvalidCriteriaWeight));
            });

            job.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobName, x.GetReference()));
            job.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobDescription, x.GetReference()));
            job.RuleFor(x => x.DeadLine).Must(x => x.Date < DateTime.Now).WithState(x => AddCommandErrorObject(EntityError.DeadLineLessDateNow, x.GetReference()));
            job.RuleFor(x => x.DeadLine).Must(x => x != DateTime.MinValue).WithState(x => AddCommandErrorObject(EntityError.InvalidDeadLine, x.GetReference()));
            job.RuleFor(x => x.DeadLine).Must(x => x.Date <= DateTime.Now.AddDays(30)).WithState(x => AddCommandErrorObject(EntityError.JobDeadLineGreaterThan30Days, x.GetReference()));
        });
    }
}

