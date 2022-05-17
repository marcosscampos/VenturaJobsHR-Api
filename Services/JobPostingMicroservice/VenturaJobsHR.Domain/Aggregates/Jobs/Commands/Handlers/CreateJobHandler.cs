using MediatR;
using MongoDB.Bson;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class CreateJobHandler : BaseJobHandler, IRequestHandler<CreateJobCommand, Unit>
{

    public CreateJobHandler(IJobRepository jobRepository, INotificationHandler notification, IMediator mediator, ICacheService cacheService)
        : base(notification, jobRepository, cacheService, mediator)
    { }

    public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        if (!await ValidateItems(request)) return Unit.Value;

        var jobList = new List<Job>();

        foreach (var item in request.JobList)
        {
            var CreatedJob = CreateJob(item);

            if (!Notification.HasErrorNotifications(item.GetReference()))
            {
                Notification.RaiseSuccess(item.Id, item.Name);
                jobList.Add(CreatedJob);
            }
        }

        if (jobList.Any())
        {
            await CreateJobAsync(jobList);
            await SetCache(jobList, request);
        }

        return Unit.Value;
    }

    private Job CreateJob(CreateOrUpdateJobRequest request)
    {
        var id = ObjectId.GenerateNewId().ToString();

        var salary = new Salary(request.Salary.Value);
        var location = new Location(request.Location.City, request.Location.State, request.Location.Country);
        var company = new Company(request.Company.Id, request.Company.Uid, request.Company.Name);

        var job = new Job(
            id,
            request.Name,
            request.Description,
            salary,
            location,
            company,
            request.FormOfHiring,
            JobStatusEnum.Published,
            request.FinalDate
            );

        if (request.CriteriaList.Any())
        {
            foreach (var item in request.CriteriaList)
            {
                var criteria = new Criteria(item.Name, item.Description, item.Profiletype, item.Weight);
                job.AddCriteria(criteria);
            }
        }

        return job;
    }
}
