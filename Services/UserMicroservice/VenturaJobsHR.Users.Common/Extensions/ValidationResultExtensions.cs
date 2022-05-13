using FluentValidation.Results;
using VenturaJobsHR.Users.Common.Exceptions;

namespace VenturaJobsHR.Users.Common.Extensions;

public static class ValidationResultExtensions
{
    public static void HandleResult(this ValidationResult validationResult)
    {
        if (!validationResult.IsValid)
            throw new BadRequestException(validationResult.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage));
    }
}
