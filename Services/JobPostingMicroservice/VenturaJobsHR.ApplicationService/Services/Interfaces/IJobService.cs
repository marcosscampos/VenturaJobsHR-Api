using System.Collections.Generic;
using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

namespace VenturaJobsHR.Application.Services.Interfaces;

public interface IJobService
{
    Task<IList<Job>> GetAll();
    Task CreateJob(CreateJobCommand command);
    Task<Job> GetById(string id);
    Task UpdateJob(UpdateJobCommand command);
    Task LogicalDeleteJob(ActiveJobRecord job);
    Task DeleteJob(string id);
    Task<List<Job>> GetAllJobsByCriteria(SearchJobsQuery query);
    Task<Pagination<Job>> GetAllJobsByCriteriaAndPaged(SearchJobsQuery query);
}
