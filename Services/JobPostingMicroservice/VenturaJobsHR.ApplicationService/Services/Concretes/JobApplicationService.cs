using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.Common.Exceptions;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

namespace VenturaJobsHR.Application.Services.Concretes;

public class JobApplicationService : ApplicationServiceBase, IJobApplicationService
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IJobRepository _jobRepository;
    private readonly IMediator _mediator;

    public JobApplicationService(
        IJobApplicationRepository applicationRepository,
        INotificationHandler notification,
        IJobRepository jobRepository, 
        IMediator mediator, 
        IHttpContextAccessor httpContext, 
        IUserRepository userRepository) : base(notification)
    {
        _applicationRepository = applicationRepository;
        _jobRepository = jobRepository;
        _mediator = mediator;
        _httpContext = httpContext;
        _userRepository = userRepository;
    }

    public async Task ApplyToJob(CreateJobApplicationCommand job)
        => await _mediator.Send(job);

    public async Task<Pagination<GetApplicationJobsRecord>> GetApplicationsFromApplicant(SearchJobsQuery query)
    {
        var userId = _httpContext.HttpContext.User.FindFirst("user_id");

        if (userId is null)
            throw new ForbiddenException("Token has expired or is invalid.");

        var user = await _userRepository.GetUserByFirebaseId(userId.Value);
        if (user is null)
            throw new NotFoundException($"User not found with ID #{user.Id}.");

        var application = await _applicationRepository.GetApplicationsByUserId(user.Id);

        query.JobsId = new List<string>();
        query.JobsId.AddRange(application.Select(x => x.JobId).ToList());
        var jobs = await _jobRepository.GetJobsPaged(query.BuildFilter(), query.Pagination);

        var jobsList = CreateApplicationJobsList(jobs.Data);

        return new Pagination<GetApplicationJobsRecord>
        {
            Data = jobsList,
            CurrentPage = jobs.CurrentPage,
            PageSize = jobs.PageSize,
            Total = jobs.Total
        };
    }
}
