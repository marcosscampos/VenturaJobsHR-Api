using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class UpdateJobCommand : BaseJobCommand, IRequest<Unit>
{
    [JsonIgnore]
    public List<Job> EntityList { get; set; }

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

            job.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobName, ""));
            job.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobDescription, $"{x.Name}"));
            job.RuleFor(x => x.FinalDate).Must(x => x != DateTime.MinValue).WithState(x => AddCommandErrorObject(EntityError.InvalidFinalDate, $"{x.Name}"));
            job.RuleFor(x => x).Must(x => x.FinalDate >= DateTime.Now.Date).WithState(x => AddCommandErrorObject(EntityError.FinalDateLessCreationDate, $"{x.Name}"));
            job.RuleFor(x => x.FinalDate).Must(x => x.Date >= DateTime.Now.Date).WithState(x => AddCommandErrorObject(EntityError.FinalDateLessDateNow, $"{x.Name}"));
        });
    }
}