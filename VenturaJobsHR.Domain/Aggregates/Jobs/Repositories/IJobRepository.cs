using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.SeedWork.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids);
}
