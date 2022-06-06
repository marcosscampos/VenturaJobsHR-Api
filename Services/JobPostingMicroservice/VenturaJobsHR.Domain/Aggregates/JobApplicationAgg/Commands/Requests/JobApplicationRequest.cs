using FluentValidation;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;

public class JobApplicationRequest
{
    public string UserId { get; set; }
    public string JobId { get; set; }
    public List<JobCriteriaRequest> CriteriaList { get; set; }
    public string GetReference()
        => $"JOBAPPLICATION-#{UserId.ToUpper()}#{JobId.ToUpper()}";
}

public class JobApplicationValidator : BaseValidator<JobApplicationRequest>
{
    public JobApplicationValidator(string reference)
    {
        RuleFor(x => x.UserId).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.JobApplicationUserIdInvalid, reference));
        RuleFor(x => x.JobId).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.JobApplicationJobIdInvalid, reference));
    }
}
