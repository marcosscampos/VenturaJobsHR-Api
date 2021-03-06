using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
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

    public async Task UpdateRangeAsync(List<T> entities)
    {
        var updates = (from entity in entities
            let propertyInfo = entity.GetType().GetProperty("Id")
            let value = propertyInfo.GetValue(entity, null)
            let filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(value.ToString()))
            select new ReplaceOneModel<T>(filter, entity)).Cast<WriteModel<T>>().ToList();

        await Collection.BulkWriteAsync(updates, new BulkWriteOptions() { IsOrdered = false });
    }

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

    public async Task<Pagination<T>> FindByFilterAsync(Expression<Func<T, bool>> filter, Page pagination)
    {
        var dataResponse = await Collection.Find(filter).Skip(pagination.StartIndex).Limit(pagination.Length)
            .ToListAsync();

        var count = await Collection.CountDocumentsAsync(filter);

        return new Pagination<T>(data: dataResponse, currentPage: pagination.CurrentPage, pageSize: pagination.Length,
            items: count);
    }

    public async Task BulkInsertAsync(List<T> entities)
    {
        var inserts = new List<WriteModel<T>>();

        foreach (var entity in entities)
        {
            inserts.Add(new InsertOneModel<T>(entity));
        }

        await Collection.BulkWriteAsync(inserts, new() { IsOrdered = false });
    }
}