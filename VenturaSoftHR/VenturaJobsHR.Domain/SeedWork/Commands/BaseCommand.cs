
using FluentValidation.Results;
using Newtonsoft.Json;

namespace VenturaJobsHR.Domain.SeedWork.Commands;

public abstract class BaseCommand
{
    [JsonIgnore]
    public ValidationResult? ValidationResult { get; set; }

    public abstract bool IsValid();
}

public abstract class BaseListCommand : List<BaseCommand> { }