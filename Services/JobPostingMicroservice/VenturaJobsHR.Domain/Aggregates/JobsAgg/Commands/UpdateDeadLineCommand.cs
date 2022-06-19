using FluentValidation;
using MediatR;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;

public class UpdateDeadLineCommand : BaseCommand, IRequest<Unit>
{
    public string Id { get; set; }
    public DateTime DeadLine { get; set; }

    public override bool IsValid()
    {
        ValidationResult = new UpdateDeadLineValidator().Validate(this);

        return ValidationResult.IsValid;
    }
}

public class UpdateDeadLineValidator : BaseValidator<UpdateDeadLineCommand>
{
    public UpdateDeadLineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithState(x => AddCommandErrorObject(EntityError.JobIdInvalid, string.Empty));
        
        RuleFor(x => x.DeadLine)
            .Must(x => x.Date >= DateTime.Now.Date)
            .WithState(x => AddCommandErrorObject(EntityError.DeadLineLessDateNow, string.Empty));
    }
}