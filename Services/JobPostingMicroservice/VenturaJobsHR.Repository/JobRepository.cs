using MongoDB.Driver;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Repository.Context;

namespace VenturaJobsHR.Repository;

public class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(IMongoContext context) : base(context)
    {
    }

    public async Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids)
        => await Collection.Find(x => ids.Contains(x.Id)).ToListAsync();
}
