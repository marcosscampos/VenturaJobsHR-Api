using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Applications;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.Common.Exceptions;
using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

namespace VenturaJobsHR.Application.Services.Concretes;

public class JobService : ApplicationServiceBase, IJobService
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserRepository _userRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IMediator _mediator;

    public JobService(IJobRepository jobRepository,
        IMediator mediator,
        INotificationHandler notification,
        IHttpContextAccessor httpContext,
        IJobApplicationRepository jobApplicationRepository,
        IUserRepository userRepository) : base(notification)
    {
        _jobRepository = jobRepository;
        _mediator = mediator;
        _httpContext = httpContext;
        _jobApplicationRepository = jobApplicationRepository;
        _userRepository = userRepository;
    }

    public async Task CreateJob(CreateJobCommand command)
        => await _mediator.Send(command);

    public async Task<GetJobsRecord> GetById(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var job = await _jobRepository.GetByIdAsync(id);

        if (job is null)
            throw new NotFoundException($"Job not found with id #{id}");

        var jobRecord = CreateObject(job);
        return jobRecord;
    }

    public async Task<JobReportRecord> GetJobReport(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var job = await _jobRepository.GetByIdAsync(id);
        if (job is null)
            throw new NotFoundException($"Job not found with id #{id}");

        var applications = await _jobApplicationRepository.GetApplicationsByJobId(job.Id);
        
        var jobAverage =
            job.CriteriaList.Sum(criteria => Job.GetProfileTypeBy(criteria.Profiletype) * criteria.Weight) /
            (double)job.CriteriaList.Sum(x => x.Weight);

        var jobProfileAverage = Math.Round(jobAverage, 2);

        var users = await _userRepository.GetUsersByIdList(applications.Select(x => x.UserId).ToList());

        var userValueList = (from app in applications
            let user = users.FirstOrDefault(x => x.Id == app.UserId)
            let average = app.CriteriaList.Sum(x =>
                              Job.GetProfileTypeBy(x.ProfileType) *
                              job.CriteriaList.FirstOrDefault(p => p.Id == x.CriteriaId)!.Weight) /
                          (double)job.CriteriaList.Sum(x => x.Weight)
            let profileAverage = Math.Round(average, 2)
            select new UserValueRecord(user.Name, profileAverage)).ToList();

        var jobConsolidated = new JobReportRecord(jobProfileAverage, userValueList);
        return jobConsolidated;
    }

    public async Task UpdateJob(UpdateJobCommand command)
    {
        var entity = await _jobRepository.GetAllJobsByIdAsync(command.JobList.Select(x => x.Id).ToList());
        var jobsToCreate = command.JobList.Where(x => !entity.Select(x => x.Id).Contains(x.Id)).ToList();
        command.JobList = command.JobList.Where(x => !jobsToCreate.Contains(x)).ToList();
        command.EntityList = entity.ToList();

        if (command.EntityList.Any())
            await _mediator.Send(command);

        if (jobsToCreate.Any())
            await CreateJob(new CreateJobCommand() { JobList = jobsToCreate });
    }

    public async Task CloseJobPosting(CloseJobRecord jobRecord)
    {
        var jobToClose = await _jobRepository.GetByIdAsync(jobRecord.Id);
        if (jobToClose is null)
            throw new NotFoundException($"Job not found with id #{jobRecord.Id}");

        jobToClose.Status = JobStatusEnum.Closed;

        await _jobRepository.UpdateAsync(jobToClose);
    }

    public async Task UpdateDeadLineJobPosting(UpdateDeadLineCommand command)
    {
        if (!ObjectId.TryParse(command.Id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        await _mediator.Send(command);
    }

    public async Task<Pagination<GetJobsRecord>> GetAllJobsByCriteriaAndPaged(SearchJobsQuery query)
    {
        var jobsToExpire = new List<Job>();
        var jobs = await _jobRepository.FindByFilterAsync(query.BuildFilter(), query.Pagination);
        foreach (var item in jobs.Data.Where(item =>
                     item.DeadLine < new DateTimeWithZone(DateTime.Now).LocalTime &&
                     item.Status == JobStatusEnum.Published))
        {
            item.Status = JobStatusEnum.Expired;
            jobsToExpire.Add(item);
        }

        if (jobsToExpire.Any())
            await _jobRepository.UpdateRangeAsync(jobsToExpire);

        var recordList = CreateList(jobs.Data)
            .Where(x => x.Status != JobStatusEnum.Expired && x.Status != JobStatusEnum.Closed).ToList();

        var newJobList = new Pagination<GetJobsRecord>
        {
            Data = recordList,
            CurrentPage = jobs.CurrentPage,
            PageSize = jobs.PageSize,
            Total = jobs.Total
        };

        return newJobList;
    }
    
    public async Task<Pagination<GetJobsRecord>> GetJobsByToken(SearchJobsQuery query)
    {
        var userId = _httpContext.HttpContext.User.FindFirst("user_id");

        if (userId is null)
            throw new ForbiddenException("Token has expired or is invalid.");

        query.Uid = userId.Value;
        var jobs = await _jobRepository.GetJobsByFirebaseTokenAndPaged(query.BuildFilter(), query.Pagination);
        var jobList = CreateList(jobs.Data);
        
        return new Pagination<GetJobsRecord>
        {
            Data = jobList,
            CurrentPage = jobs.CurrentPage,
            PageSize = jobs.PageSize,
            Total = jobs.Total
        };
    }

    public async Task LogicalDeleteJob(ActiveJobRecord job)
    {
        if (!ObjectId.TryParse(job.Id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var jobToDelete = await _jobRepository.GetByIdAsync(job.Id);

        if (jobToDelete is null)
            throw new NotFoundException($"Job not found with id #{job.Id}");

        jobToDelete.Active = job.Active;

        await _jobRepository.UpdateAsync(jobToDelete);
    }

    public async Task<IList<ApplicationResponse>> GetApplicationsByToken(string id)
    {
        var userId = _httpContext.HttpContext.User.FindFirst("user_id");

        if (userId is null)
            throw new ForbiddenException("Token has expired or is invalid.");

        var user = await _userRepository.GetUserByFirebaseId(userId.Value);

        if (user is null)
            throw new NotFoundException($"User not found with ID #{user.Id}.");

        var applications = await _jobApplicationRepository.GetApplicationsByUserId(user.Id);

        var job = await _jobRepository.GetByIdAsync(id);
        var applicationList = CreateJobApplicationList(applications.ToList(), job, user);

        return applicationList;
    }
}