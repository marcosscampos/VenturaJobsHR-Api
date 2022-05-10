using FluentValidation;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class CompanyRequest
{
    public string Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
}

public class CompanyValidator : BaseValidator<CompanyRequest>
{
    public CompanyValidator(string reference)
    {
        RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.CompanyInvalidName, reference));
    }
}