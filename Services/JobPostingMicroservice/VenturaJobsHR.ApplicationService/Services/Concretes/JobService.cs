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
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
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


    public async Task<IList<GetJobsRecord>> GetAll()
    {
        var jobs = await _jobRepository.GetAllAsync();

        var recordList = CreateList(jobs.ToList());
        return recordList;
    }

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

    public async Task DeleteJob(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        await _jobRepository.DeleteAsync(id);
    }

    public async Task<List<GetJobsRecord>> GetAllJobsByCriteria(SearchJobsQuery query)
    {
        var jobs = await _jobRepository.FindAsync(query.BuildFilter());
        var recordList = CreateList(jobs.ToList());

        return recordList;
    }

    public async Task<Pagination<GetJobsRecord>> GetAllJobsByCriteriaAndPaged(SearchJobsQuery query)
    {
        var jobs = await _jobRepository.FindByFilterAsync(query.BuildFilter(), query.Pagination);
        var recordList = CreateList(jobs.Data);

        var newJobList = new Pagination<GetJobsRecord>
        {
            Data = recordList,
            CurrentPage = jobs.CurrentPage,
            PageSize = jobs.PageSize,
            Total = jobs.Total
        };

        return newJobList;
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

    public async Task<IList<GetJobsRecord>> GetJobsByToken()
    {
        var userId = _httpContext.HttpContext.User.FindFirst("user_id");

        if (userId is null)
            throw new ForbiddenException("Token has expired or is invalid.");

        var jobs = await _jobRepository.GetJobsByFirebaseToken(userId.Value);
        var jobList = CreateList(jobs.ToList());

        return jobList;
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
