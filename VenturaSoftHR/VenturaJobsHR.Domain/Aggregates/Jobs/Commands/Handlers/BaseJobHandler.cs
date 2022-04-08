using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.SeedWork.Handlers;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class BaseJobHandler : BaseRequestHandler
{
    private readonly IMediator _mediator;
    protected BaseJobHandler(INotificationHandler notification, IMediator mediator) : base(notification)
    {
        _mediator = mediator;
    }

    protected async Task CreateJob(Job job)
    {
        var obj = job.ProjectedAs<JobsCreatedEvent>();
        await _mediator.Publish(obj);
    }

    protected async Task UpdateJob(Job job)
    {
        var obj = job.ProjectedAs<JobsUpdatedEvent>();
        await _mediator.Publish(obj);
    }
}
