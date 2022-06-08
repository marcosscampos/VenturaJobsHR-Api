using MongoDB.Driver;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Repository.Context;

namespace VenturaJobsHR.Repository;

public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(IMongoContext context) : base(context)
    {
    }

    public async Task<IList<JobApplication>> GetApplicationsByUserId(string userId)
        => await Collection.Find(x => x.UserId == userId).ToListAsync();

    public async Task<List<JobApplication>> GetApplicationsByJobId(string jobId)
        => await Collection.Find(x => x.JobId == jobId).ToListAsync();
}