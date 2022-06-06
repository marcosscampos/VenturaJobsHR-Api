using FluentValidation;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;

public class LocationRequest
{
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}

public class LocationValidator : BaseValidator<LocationRequest>
{
    public LocationValidator(string reference)
    {
        RuleFor(x => x.City).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidLocation, reference));
        RuleFor(x => x.State).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidLocation, reference));
        RuleFor(x => x.Country).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidLocation, reference));
    }
}