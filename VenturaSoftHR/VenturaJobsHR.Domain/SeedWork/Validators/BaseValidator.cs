using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using VenturaJobsHR.Domain.SeedWork.Commands;

namespace VenturaJobsHR.Domain.SeedWork.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T> where T : class
{
    protected CommandErrorObject AddCommandErrorObject(Enum error, string reference = null)
    {
        return new CommandErrorObject(error, reference);
    }
}
