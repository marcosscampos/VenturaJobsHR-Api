using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids);
    Task<IList<Job>> GetJobsByFirebaseToken(string userId);
}
