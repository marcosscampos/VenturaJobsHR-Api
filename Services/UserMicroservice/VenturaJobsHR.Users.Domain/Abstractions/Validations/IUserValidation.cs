using FluentValidation;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Domain.Abstractions.Validations;

public interface IUserValidation : IValidator<User>
{
}
