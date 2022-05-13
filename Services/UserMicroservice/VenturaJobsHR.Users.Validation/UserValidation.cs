using FluentValidation;
using VenturaJobsHR.Users.Domain.Abstractions.Validations;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Validation;

public class UserValidation : AbstractValidator<User>, IUserValidation
{
    public UserValidation()
    {

    }
}
