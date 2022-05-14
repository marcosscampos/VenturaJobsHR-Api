using MediatR;
using System.Diagnostics.CodeAnalysis;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public abstract class BaseJobCommand : BaseCommand, IRequest<Unit>
{
    public List<CreateOrUpdateJobRequest> JobList { get; set; }

    public BaseJobCommand()
    {
        JobList = new List<CreateOrUpdateJobRequest>();
    }
}
