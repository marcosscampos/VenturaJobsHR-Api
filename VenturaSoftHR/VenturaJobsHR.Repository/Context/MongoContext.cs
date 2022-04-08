using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VenturaJobsHR.Common.Exceptions;

namespace VenturaJobsHR.Repository.Context;

public class MongoContext : IMongoContext
{
    public IMongoDatabase Database { get; set; }
    private readonly MongoClient _client;

    public MongoContext(string connectionString, string databaseName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidParameterException("The parameter 'connectionString' is not found.");

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new InvalidParameterException("The parameter 'databaseName' is not found.");

        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(10);
        settings.MaxConnectionLifeTime = TimeSpan.FromMinutes(1);
        
        _client = new MongoClient(settings);
        Database = _client.GetDatabase(databaseName);
    }

    public async Task<IClientSessionHandle> BeginTransactionAsync()
        => await _client.StartSessionAsync();

    public IMongoCollection<T> GetCollection<T>(string name)
        => Database.GetCollection<T>(name);
}

public interface IMongoContext
{
    IMongoDatabase Database { get; set; }
    Task<IClientSessionHandle> BeginTransactionAsync();
    IMongoCollection<T> GetCollection<T>(string name);
}