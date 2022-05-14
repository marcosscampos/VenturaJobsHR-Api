using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids);
}
