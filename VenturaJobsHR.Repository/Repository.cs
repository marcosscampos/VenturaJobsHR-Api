using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using VenturaJobsHR.Domain.SeedWork.Repositories;
using VenturaJobsHR.Repository.Context;

namespace VenturaJobsHR.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly IMongoContext _mongoContext;
    protected readonly IMongoCollection<T> Collection;

    public Repository(IMongoContext context)
    {
        _mongoContext = context;
        Collection = _mongoContext.GetCollection<T>(typeof(T).Name);
    }

    public async Task CreateAsync(T entity) => await Collection.InsertOneAsync(entity);

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        await Collection.DeleteOneAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await Collection.Find(x => true).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
        => await Collection.Find(filter).ToListAsync();

    public async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        return await Collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        var propertyInfo = entity.GetType().GetProperty("Id");
        var value = propertyInfo.GetValue(entity, null);
        var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(value.ToString()));

        await Collection.ReplaceOneAsync(filter, entity);
    }
}
