using FluentValidation;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;

public class JobCriteriaRequest
{
    public string CriteriaId { get; set; }
    public ProfileTypeEnum ProfileType { get; set; }

    public string GetReference()
        => $"CRITERIA-#{CriteriaId}#{ProfileType}";
}

public class CreateJobCriteriaValidator : BaseValidator<JobCriteriaRequest>
{
    public CreateJobCriteriaValidator(string reference)
    {
        RuleFor(x => x.CriteriaId).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.JobApplicationCriteriaNotNull, reference));
    }
}
