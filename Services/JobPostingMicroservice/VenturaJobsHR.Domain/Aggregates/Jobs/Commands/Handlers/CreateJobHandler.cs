using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Message.Dto.Job;
using VenturaJobsHR.Message.Interface;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class CreateJobHandler : BaseJobHandler, IRequestHandler<CreateJobCommand, Unit>
{
    private readonly IConfiguration _configuration;
    public CreateJobHandler(IJobRepository jobRepository, INotificationHandler notification, IMediator mediator, IBusService busService, IConfiguration configuration)
        : base(notification, mediator, jobRepository, busService)
    {
        _configuration = configuration;
    }

    public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request))
            return Unit.Value;

        var jobListDto = new List<JobDto>();

        foreach (var item in request.JobList)
        {
            var CreatedJob = CreateJob(item);

            if (!Notification.HasErrorNotifications(item.GetReference()))
            {
                Notification.RaiseSuccess(item.Id, item.Name);
                jobListDto.Add(CreatedJob.ProjectedAs<JobDto>());
            }
        }

        if(jobListDto.Any())
        {
            await PublishToQueue(jobListDto, _configuration["MessagesConfiguration:Queues:Jobs"]);
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
            request.Status,
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
