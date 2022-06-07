using MediatR;
using MongoDB.Bson;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Handlers;

public class UpdateJobHandler : BaseJobHandler, IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;

    public UpdateJobHandler(INotificationHandler notification, IJobRepository jobRepository, IMediator mediator,
        ICacheService cacheService) : base(notification, jobRepository, cacheService, mediator)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request)) return Unit.Value;

        var jobList = new List<Job>();

        foreach (var item in request.JobList)
        {
            var updatedJob = request.EntityList.FirstOrDefault(x => x.Id == item.Id);

            if (updatedJob == null) continue;

            await UpdateJob(item, updatedJob);

            if (!Notification.HasErrorNotifications(updatedJob.Id))
            {
                Notification.RaiseSuccess(updatedJob.Id, updatedJob.Description);
                jobList.Add(updatedJob);
            }
        }

        if (jobList.Any())
        {
            await UpdateJobAsync(jobList);
            await SetCache(jobList, request);
        }

        return Unit.Value;
    }

    private async Task UpdateJob(CreateOrUpdateJobRequest request, Job job)
    {
        var databaseJob = await _jobRepository.GetByIdAsync(request.Id);

        var id = ObjectId.GenerateNewId().ToString();
        var salary = job.Salary;
        var location = job.Location;
        var company = job.Company;
        var criteriaToRemoveList = new List<Criteria>();

        salary.Update(request.Salary.Value);
        location.Update(request.Location.City, request.Location.State, request.Location.Country);
        company.Update(request.Company.Id, request.Company.Uid, request.Company.Name);

        job.Update(
            request.Id,
            request.Name,
            request.Description,
            salary,
            location,
            company,
            request.Status,
            request.OccupationArea,
            request.FormOfHiring,
            request.DeadLine
        );

        if (request.CriteriaList.Any())
        {
            var criteriaToProcess = request.CriteriaList.Select(x => new { Id = x.Id }).ToList();
            var criteriaToAdd = criteriaToProcess.Except(databaseJob.CriteriaList.Select(x => new { Id = x.Id }));

            if (criteriaToAdd.Any())
            {
                foreach (var criteria in request.CriteriaList.Select(item
                             => new Criteria(
                                 id,
                                 item.Name,
                                 item.Description,
                                 item.Profiletype,
                                 item.Weight)))
                {
                    job.AddCriteria(criteria);
                }
            }

            foreach (var item in databaseJob.CriteriaList)
            {
                var exists = job.CriteriaList.Exists(x => x.Id == item.Id);
                if (!exists) continue;
                
                var criteria = job.CriteriaList.FirstOrDefault(x => x.Id == item.Id);
                var criteriaRequest = request.CriteriaList.FirstOrDefault(x => x.Id == item.Id);
                if (criteriaRequest != null && criteria != null)
                {
                    criteria.Update(
                        criteriaRequest.Name, 
                        criteriaRequest.Description, 
                        criteriaRequest.Profiletype,
                        criteriaRequest.Weight);
                }
            }

            criteriaToRemoveList.AddRange(
                from item in job.CriteriaList 
                select databaseJob.CriteriaList.FirstOrDefault(x => x.Id == item.Id) 
                into criteria where criteria != null 
                let removeCriteria = request.CriteriaList.FirstOrDefault(x => x.Id == criteria.Id) 
                where removeCriteria is null select criteria);

            if (criteriaToRemoveList.Any())
                criteriaToRemoveList.ForEach(x =>
                {
                    job.CriteriaList.RemoveAll(p => p.Id == x.Id);
                });
        }
    }
}