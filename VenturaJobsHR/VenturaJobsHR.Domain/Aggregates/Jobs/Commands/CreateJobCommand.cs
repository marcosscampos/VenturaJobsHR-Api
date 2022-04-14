using FluentValidation;
using MediatR;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

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
        RuleForEach(x => x.Job).ChildRules(job =>
        {
            job.RuleFor(x => x.Salary).NotNull().WithState(x => AddCommandErrorObject(EntityError.InvalidSalary, $"{x.Name}"));
            job.RuleFor(x => x.Salary.Value).NotEqual(0).WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, $"{x.Name}"));
            job.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobName, ""));
            job.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobDescription, $"{x.Name}"));
            job.RuleFor(x => x.FinalDate).Must(x => x != DateTime.MinValue).WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, $"{x.Name}"));
        });
    }
}

