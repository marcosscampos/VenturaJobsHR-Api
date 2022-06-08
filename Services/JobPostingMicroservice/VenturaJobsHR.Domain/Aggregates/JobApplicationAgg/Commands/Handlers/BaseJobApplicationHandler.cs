using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Handlers;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Events;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Handlers;

public abstract class BaseJobApplicationHandler : BaseRequestHandler
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ICacheService _cacheService;
    private readonly IMediator _mediator;
    protected BaseJobApplicationHandler(INotificationHandler notification,
        IMediator mediator, ICacheService cacheService,
        IJobApplicationRepository applicationRepository, IJobRepository jobRepository) : base(notification)
    {
        _mediator = mediator;
        _cacheService = cacheService;
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
    }

    protected async Task CreateApplicationAsync(JobApplication application)
    {
        await _applicationRepository.CreateAsync(application);
        await _mediator.Publish(application.ProjectedAs<JobApplicationCreatedEvent>());
    }

    protected async Task RemoveDuplicatesFromCache(BaseJobApplicationCommand command)
    {
        if (command.Application is null) return;

        JobApplicationRequest? applicationToRemove = null;

        if (await _cacheService.ExistsAsync(command.Application.GetReference(), command.Application))
            applicationToRemove = command.Application;

        if (applicationToRemove != null)
            command.Application = null;
    }

    protected async Task<bool> IsDuplicated(object command, string reference)
    {
        if (command is null) return false;

        return await _cacheService.ExistsAsync(reference, command);
    }

    protected async Task SetCache(JobApplication application)
    {
        var job = await _jobRepository.GetByIdAsync(application.JobId);

        object app = new { UserId = application.UserId, JobId = application.JobId };

        await _cacheService.InsertOrUpdateAsync(application.GetKeyCache(), app, job.DeadLine);
    }
}
