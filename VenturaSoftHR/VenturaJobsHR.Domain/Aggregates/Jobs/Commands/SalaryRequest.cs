using FluentValidation;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class SalaryRequest
{
    public decimal Value { get; set; }
}

public class SalaryValidator : BaseValidator<Salary>
{
    public SalaryValidator(string reference)
    {
        RuleFor(x => x.Value).Must(x => x > 0)
            .WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, reference));
    }
}