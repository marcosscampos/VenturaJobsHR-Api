using MediatR;
using Microsoft.Extensions.Configuration;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Message.Dto.Job;
using VenturaJobsHR.Message.Interface;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class UpdateJobHandler : BaseJobHandler, IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;
    private readonly IConfiguration _configuration;

    public UpdateJobHandler(INotificationHandler notification, IJobRepository jobRepository, IMediator mediator, IBusService busService, IConfiguration configuration) 
        : base(notification, mediator, jobRepository, busService)
    {
        _configuration = configuration;
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request))
            return Unit.Value;

        var jobListDto = new List<JobDto>();

        foreach (var item in request.JobList)
        {
            var returnedJob = await _jobRepository.GetByIdAsync(item.Id);
            var updatedJob = request.EntityList.FirstOrDefault(x => x.Id == item.Id);

            if (updatedJob == null) continue;

            UpdateJob(item, updatedJob);

            if (!Notification.HasErrorNotifications(updatedJob.Id))
            {
                Notification.RaiseSuccess(updatedJob.Id, updatedJob.Description);
                var job = updatedJob.ProjectedAs<JobDto>();
                jobListDto.Add(job);
            }
        }

        if(jobListDto.Any())
        {
            await PublishToQueue(jobListDto, _configuration["MessagesConfiguration:Queues:Jobs"]);
        }

        return Unit.Value;
    }


    private void UpdateJob(CreateOrUpdateJobRequest request, Job job)
    {
        var salary = new Salary(request.Salary.Value);
        var location = new Location(request.Location.City, request.Location.State, request.Location.Country);
        var company = new Company(request.Company.Id, request.Company.Uid, request.Company.Name);

        job.Update(
            request.Id,
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
    }
}
