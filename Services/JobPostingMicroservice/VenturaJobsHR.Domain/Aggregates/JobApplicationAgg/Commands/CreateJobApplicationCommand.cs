using FluentValidation;
using MediatR;
using VenturaJobsHR.CrossCutting.Enums;
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
        RuleFor(x => x.Application.UserId).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.JobApplicationUserIdInvalid, x.Application.GetReference()));
        RuleFor(x => x.Application.JobId).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.JobApplicationJobIdInvalid, x.Application.GetReference()));
        RuleFor(x => x.Application.CriteriaList).NotNull().Must(x => x.Count > 0).WithState(x => AddCommandErrorObject(EntityError.JobApplicationCriteriaNotNull, x.Application.GetReference()));

        RuleForEach(x => x.Application.CriteriaList).ChildRules(criteria =>
        {
            criteria.RuleFor(x => x).SetValidator(x => new CreateJobCriteriaValidator(x.GetReference())).DependentRules(() =>
            {
                criteria.RuleFor(x => x).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobApplicationCriteriaNotNull, x.GetReference()));
            });
        });
    }
}
