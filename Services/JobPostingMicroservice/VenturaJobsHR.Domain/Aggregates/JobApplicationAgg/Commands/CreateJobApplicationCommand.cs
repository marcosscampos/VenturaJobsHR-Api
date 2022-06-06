using FluentValidation;
using MediatR;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;

public class CreateJobApplicationCommand : BaseJobApplicationCommand, IRequest<Unit>
{
    public override bool IsValid()
    {
        ValidationResult = new CreateJobApplicationValidator().Validate(this);

        return ValidationResult.IsValid;
    }
}

public class CreateJobApplicationValidator : BaseValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationValidator()
    {
        RuleFor(x => x.Application).ChildRules(app =>
        {
            app.RuleFor(x => x).SetValidator(x => new JobApplicationValidator(x.GetReference()));
        });
    }
}
