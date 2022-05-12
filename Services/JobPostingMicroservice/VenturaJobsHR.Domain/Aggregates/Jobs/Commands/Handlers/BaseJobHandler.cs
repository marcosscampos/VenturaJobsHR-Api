using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Domain.SeedWork.Handlers;
using VenturaJobsHR.Message.Dto.Common;
using VenturaJobsHR.Message.Dto.Job;
using VenturaJobsHR.Message.Interface;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class BaseJobHandler : BaseRequestHandler
{
    private readonly IJobRepository _jobRepository;
    private readonly IBusService _busService;
    private readonly IMediator _mediator;

    protected BaseJobHandler(INotificationHandler notification, IMediator mediator, IJobRepository repository, IBusService busService) : base(notification)
    {
        _mediator = mediator;
        _jobRepository = repository;
        _busService = busService;
    }

    protected async Task PublishToQueue(List<JobDto> jobList, string queue)
    {
        var obj = new MessageQueueDto<JobDto>(jobList);

        await _busService.SendMessageToQueueAsync(obj, queue);
        await _mediator.Publish(obj.ProjectedAs<JobsCreatedEvent>());
    }
}
