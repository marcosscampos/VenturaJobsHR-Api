using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Events;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Handlers;

public class UpdateDeadLineJobHandler : BaseJobHandler, IRequestHandler<UpdateDeadLineCommand, Unit>
{
    private readonly IJobRepository _jobRepository;
    private readonly IMediator _mediator;

    public UpdateDeadLineJobHandler(INotificationHandler notification, IJobRepository jobRepository,
        ICacheService cacheService, IMediator mediator) : base(notification, jobRepository, cacheService, mediator)
    {
        _jobRepository = jobRepository;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateDeadLineCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request)) return Unit.Value;
        
        if (!Notification.HasErrorNotifications())
        {
            var job = await _jobRepository.GetByIdAsync(request.Id);
            job.DeadLine = request.DeadLine;

            if (job.Status == JobStatusEnum.Expired)
                job.Status = JobStatusEnum.Published;

            await _jobRepository.UpdateAsync(job);
            await _mediator.Publish(job.ProjectedAs<JobsUpdatedEvent>());
        }

        return Unit.Value;
    }
}