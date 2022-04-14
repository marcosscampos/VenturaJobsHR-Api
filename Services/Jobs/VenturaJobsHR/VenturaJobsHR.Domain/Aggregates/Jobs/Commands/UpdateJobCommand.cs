using FluentValidation;
using MediatR;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class UpdateJobCommand : BaseJobCommand, IRequest<Unit>
{
    public override bool IsValid()
    {
        ValidationResult = new UpdateJobValidator().Validate(this);

        return ValidationResult.IsValid;
    }
}

public class UpdateJobValidator : BaseValidator<UpdateJobCommand>
{
    public UpdateJobValidator()
    {
        RuleForEach(x => x.Job).ChildRules(job =>
        {
            job.RuleFor(x => x.Salary).NotNull().WithState(x => AddCommandErrorObject(EntityError.InvalidSalary, $"{x.Name}"));
            job.RuleFor(x => x.Salary.Value).NotEqual(0).WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, $"{x.Name}"));
            job.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobName, ""));
            job.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobDescription, $"{x.Name}"));
            job.RuleFor(x => x.FinalDate).Must(x => x != DateTime.MinValue).WithState(x => AddCommandErrorObject(EntityError.InvalidFinalDate, $"{x.Name}"));
            job.RuleFor(x => x).Must(x => x.FinalDate >= x.CreationDate).WithState(x => AddCommandErrorObject(EntityError.FinalDateLessCreationDate, $"{x.Name}"));
            job.RuleFor(x => x.FinalDate).Must(x => x.Date >= DateTime.Now.Date).WithState(x => AddCommandErrorObject(EntityError.FinalDateLessDateNow, $"{x.Name}"));
        });
    }
}