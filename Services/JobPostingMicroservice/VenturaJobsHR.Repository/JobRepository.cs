using System.Linq.Expressions;
using MongoDB.Driver;
using VenturaJobsHR.CrossCutting.Pagination;
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
    
    public async Task<Pagination<Job>> GetJobsByFirebaseTokenAndPaged(Expression<Func<Job, bool>> filter, Page pagination)
    {
        var dataResponse = await Collection.Find(filter).Skip(pagination.StartIndex).Limit(pagination.Length)
            .ToListAsync();
        var count = await Collection.CountDocumentsAsync(filter);

        return new Pagination<Job>(
            data: dataResponse, 
            currentPage: pagination.CurrentPage, 
            pageSize: pagination.Length,
            items: count);
    }
}
