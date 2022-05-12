using MassTransit;
using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Message.Dto.Common;
using VenturaJobsHR.Message.Dto.Job;

namespace VenturaJobsHR.Consumer.Consumers;

public class JobConsumer : IConsumer<MessageQueueDto<JobDto>>
{
    private readonly IMediator _mediator;
    private readonly IJobRepository _jobRepository;
    public JobConsumer(IMediator mediator, IJobRepository jobRepository)
    {
        _mediator = mediator;
        _jobRepository = jobRepository;
    }

    public async Task Consume(ConsumeContext<MessageQueueDto<JobDto>> context)
    {
        var jobsList = context.Message.ListEntity.ProjectedAs<List<Job>>();

        if(jobsList.Any())
        {
            await _jobRepository.BulkInsertAsync(jobsList);
        }

        await _mediator.Publish(context.Message.ProjectedAs<JobsConsumedEvent>());
    }
}
