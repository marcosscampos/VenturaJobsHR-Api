using MediatR;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;

public abstract class BaseJobApplicationCommand : BaseCommand, IRequest<Unit>
{
    public JobApplicationRequest Application { get; set; }

    public BaseJobApplicationCommand()
    {
        Application = new JobApplicationRequest();
    }
}
