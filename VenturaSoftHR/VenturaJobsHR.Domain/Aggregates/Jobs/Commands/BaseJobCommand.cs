using MediatR;
using System.Diagnostics.CodeAnalysis;
using VenturaJobsHR.Domain.SeedWork.Commands;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public abstract class BaseJobCommand : BaseCommand, IRequest<Unit>
{
    public List<CreateOrUpdateJobRequest> Job { get; set; }

    public BaseJobCommand()
    {
        Job = new List<CreateOrUpdateJobRequest>();
    }
}
