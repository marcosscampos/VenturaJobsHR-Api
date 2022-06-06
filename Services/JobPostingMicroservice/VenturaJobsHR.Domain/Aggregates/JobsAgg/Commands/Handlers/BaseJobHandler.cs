using MediatR;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Handlers;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Events;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Handlers;

public abstract class BaseJobHandler : BaseRequestHandler
{
    private readonly IJobRepository _jobRepository;
    private readonly ICacheService _cacheService;
    private readonly IMediator _mediator;

    protected BaseJobHandler(INotificationHandler notification, IJobRepository jobRepository, ICacheService cacheService, IMediator mediator) : base(notification)
    {
        _jobRepository = jobRepository;
        _cacheService = cacheService;
        _mediator = mediator;
    }

    protected async Task CreateJobAsync(List<Job> jobList)
    {
        await _jobRepository.BulkInsertAsync(jobList);
        await _mediator.Publish(jobList[0].ProjectedAs<JobsCreatedEvent>());
    }

    protected async Task UpdateJobAsync(List<Job> jobList)
    {
        foreach (var item in jobList)
        {
            await _jobRepository.UpdateAsync(item);
        }

        await _mediator.Publish(jobList[0].ProjectedAs<JobsUpdatedEvent>());
    }

    protected async Task<bool> ValidateItems(BaseJobCommand command)
    {
        var codes = command.JobList.Select(x => x.Company.Uid);
        var validateCodes = GetValidatedCodes(command, codes);
        if (validateCodes == null || !validateCodes.Any()) return false;

        command.JobList = command.JobList.Where(x => validateCodes.Contains(x.Company.Uid)).ToList();

        await RemoveDuplicatesFromCache(command);
        return true;
    }

    protected async Task<bool> IsDuplicated(BaseJobCommand command)
    {
        if (command.JobList is null || !command.JobList.Any()) return false;

        return await _cacheService.ExistsAsync(command.JobList[0].GetReference(), command.JobList);
    }

    protected async Task RemoveDuplicatesFromCache(BaseJobCommand command)
    {
        if (!command.JobList.Any()) return;

        var jobsToRemove = new List<CreateOrUpdateJobRequest>();
        foreach (var jobRequest in command.JobList)
        {
            if (await _cacheService.ExistsAsync($"{jobRequest.GetReference()}", jobRequest))
                jobsToRemove.Add(jobRequest);
        }

        if (jobsToRemove.Any())
            command.JobList.RemoveAll(x => jobsToRemove.Contains(x));
    }

    protected async Task SetCache(List<Job> jobList, BaseJobCommand request)
    {
        foreach (var item in jobList)
        {
            var itemCache = request.JobList.Where(x => x.Name == item.Name && x.DeadLine == item.DeadLine).FirstOrDefault();

            object app = new
            {
                Name = itemCache.Name,
                Description = itemCache.Description,
                CriteriaList = itemCache.CriteriaList,
                DeadLine = itemCache.DeadLine,
                Salary = itemCache.Salary,
                Status = itemCache.Status
            };

            await _cacheService.InsertOrUpdateAsync(item.GetKeyCache(), app, itemCache.DeadLine);
        }
    }
}
