using MongoDB.Driver;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Repository.Context;

namespace VenturaJobsHR.Repository;

public class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(IMongoContext context) : base(context)
    {
    }

    public async Task<IList<Job>> GetAllJobsByIdAsync(List<string> ids)
        => await Collection.Find(x => ids.Contains(x.Id)).ToListAsync();

    public async Task<IList<Job>> GetJobsByFirebaseToken(string userId)
        => await Collection.Find(x => x.Company.Uid == userId).ToListAsync();
}
