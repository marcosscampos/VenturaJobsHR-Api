using MediatR;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;

public abstract class BaseJobCommand : BaseCommand, IRequest<Unit>
{
    public List<CreateOrUpdateJobRequest> JobList { get; set; }

    public BaseJobCommand()
    {
        JobList = new List<CreateOrUpdateJobRequest>();
    }
}
