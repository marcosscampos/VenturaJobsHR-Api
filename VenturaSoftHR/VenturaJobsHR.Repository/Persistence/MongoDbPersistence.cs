namespace VenturaJobsHR.Repository.Persistence;

public static class MongoDbPersistence<T> where T : RepositoryMapBase
{
    public static void Configure()
    {
        var configurator = (RepositoryMapBase)Activator.CreateInstance(typeof(T));
        configurator.Configure();
    }
}
