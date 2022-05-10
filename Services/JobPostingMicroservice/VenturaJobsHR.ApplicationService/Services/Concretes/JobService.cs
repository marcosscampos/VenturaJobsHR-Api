using MediatR;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Queries;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

namespace VenturaJobsHR.Application.Services.Concretes;

public class JobService : ApplicationServiceBase, IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IMediator _mediator;

    public JobService(IJobRepository jobRepository, IMediator mediator, INotificationHandler notification) : base(notification)
    {
        _jobRepository = jobRepository;
        _mediator = mediator;
    }

    public async Task CreateJob(CreateJobCommand command)
    => await _mediator.Send(command);


    public async Task<IList<Job>> GetAll()
    {
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.ToList();
    }

    public async Task<Job> GetById(string id)
        => await _jobRepository.GetByIdAsync(id);

    public async Task UpdateJob(UpdateJobCommand command)
    {
        var entity = await _jobRepository.GetAllJobsByIdAsync(command.JobList.Select(x => x.Id).ToList());
        var jobsToCreate = command.JobList.Where(x => !entity.Select(x => x.Id).Contains(x.Id)).ToList();
        command.JobList = command.JobList.Where(x => !jobsToCreate.Contains(x)).ToList();
        command.EntityList = entity.ToList();

        if (command.EntityList.Any())
            await _mediator.Send(command);

        if (jobsToCreate.Any())
            await CreateJob(new CreateJobCommand() { JobList = jobsToCreate});
    }

    public async Task DeleteJob(string id)
        => await _jobRepository.DeleteAsync(id);

    public async Task<List<Job>> GetAllJobsByCriteria(SearchJobsQuery query)
    {
        var jobs = await _jobRepository.FindAsync(query.BuildFilter());
        return jobs.ToList();
    }

    public async Task<Pagination<Job>> GetAllJobsByCriteriaAndPaged(SearchJobsQuery query)
    {
        var jobs = await _jobRepository.FindByFilterAsync(query.BuildFilter(), query.Pagination);
        return jobs;
    }
}
