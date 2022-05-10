using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Domain.SeedWork.Handlers;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class BaseJobHandler : BaseRequestHandler
{
    private readonly IMediator _mediator;
    private readonly IJobRepository _jobRepository;

    protected BaseJobHandler(INotificationHandler notification, IMediator mediator, IJobRepository repository) : base(notification)
    {
        _mediator = mediator;
        _jobRepository = repository;
    }

    protected async Task CreateJob(Job job)
    {
        await _jobRepository.CreateAsync(job);

        var obj = job.ProjectedAs<JobsCreatedEvent>();
        await _mediator.Publish(obj);
    }

    protected async Task UpdateJob(Job job)
    {
        await _jobRepository.UpdateAsync(job);

        var obj = job.ProjectedAs<JobsUpdatedEvent>();
        await _mediator.Publish(obj);
    }
}
