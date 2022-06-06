using FluentValidation;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;

public class SalaryRequest
{
    public decimal Value { get; set; }
}

public class SalaryValidator : BaseValidator<SalaryRequest>
{
    public SalaryValidator(string reference)
    {
        RuleFor(x => x.Value).Must(x => x >= 0)
            .WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, reference));
    }
}