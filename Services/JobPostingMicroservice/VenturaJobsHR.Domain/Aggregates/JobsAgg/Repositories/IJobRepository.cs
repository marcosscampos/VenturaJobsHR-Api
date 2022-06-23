using System.Linq.Expressions;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids);
    Task<Pagination<Job>> GetJobsByFirebaseTokenAndPaged(Expression<Func<Job, bool>> filter, Page pagination);
    Task<IList<Job>> GetJobsByFirebaseToken(string userId);
}
