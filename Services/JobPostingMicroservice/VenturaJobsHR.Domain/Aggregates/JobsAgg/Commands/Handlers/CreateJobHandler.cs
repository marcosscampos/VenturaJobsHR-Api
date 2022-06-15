using MediatR;
using MongoDB.Bson;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Handlers;

public class CreateJobHandler : BaseJobHandler, IRequestHandler<CreateJobCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    public CreateJobHandler(IJobRepository jobRepository, INotificationHandler notification, IMediator mediator, ICacheService cacheService, IUserRepository userRepository)
        : base(notification, jobRepository, cacheService, mediator)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request)) return Unit.Value;

        var verifyDuplicate = new
        {
            Name = request.JobList[0].Name,
            Description = request.JobList[0].Description,
            CriteriaList = request.JobList[0].CriteriaList,
            DeadLine = request.JobList[0].DeadLine,
            Salary = request.JobList[0].Salary,
            Status = request.JobList[0].Status
        };
        
        if (await IsDuplicated(verifyDuplicate, request.JobList[0].GetReference()))
        {
            Job.JobDuplicated(Notification, request.JobList[0].GetReference());
            return Unit.Value;
        }

        var jobList = new List<Job>();

        foreach (var item in request.JobList)
        {
            var createdJob = await CreateJob(item);

            if (!Notification.HasErrorNotifications(item.GetReference()))
            {
                Notification.RaiseSuccess(item.Id, item.Name);
                jobList.Add(createdJob);
            }
        }

        if (jobList.Any())
        {
            await CreateJobAsync(jobList);
            await SetCache(jobList, request);
        }

        return Unit.Value;
    }

    private async Task<Job> CreateJob(CreateOrUpdateJobRequest request)
    {
        var id = ObjectId.GenerateNewId().ToString();
        var user = await _userRepository.GetUserByFirebaseId(request.Company.Uid);

        var salary = new Salary(request.Salary.Value);
        var location = new Location(request.Location.City, request.Location.State, request.Location.Country);
        var company = new Company(user.Id, request.Company.Uid, request.Company.Name);

        var job = new Job(
            id,
            request.Name,
            request.Description,
            salary,
            location,
            company,
            request.FormOfHiring,
            request.OccupationArea,
            JobStatusEnum.Published,
            request.DeadLine
            );

        if (request.CriteriaList.Any())
        {
            foreach (var item in request.CriteriaList)
            {
                var criteria = new Criteria(ObjectId.GenerateNewId().ToString(), item.Name, item.Description, item.Profiletype, item.Weight);
                job.AddCriteria(criteria);
            }
        }

        return job;
    }
}
