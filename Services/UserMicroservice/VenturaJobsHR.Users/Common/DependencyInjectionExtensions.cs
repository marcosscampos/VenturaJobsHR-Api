using VenturaJobsHR.Users.Domain.Abstractions.Validations;
using VenturaJobsHR.Users.Validation;

namespace VenturaJobsHR.Users.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseRepositories(dbSettings =>
        {
            dbSettings.ConnectionStringMongoDb = configuration["ConnectionStrings:MongoDb:Uri"];
            dbSettings.DatabaseName = configuration["ConnectionStrings:MongoDb:DatabaseName"];
        });

        services.UseServices();
        services.UseValidators();
        return services;
    }

    private static void UseServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        MapperFactory.Setup();
    }

    private static void UseRepositories(this IServiceCollection services, Action<IDbSettings> dbSettings)
    {
        IDbSettings configureDb = new DbSettings();
        dbSettings.Invoke(configureDb);
        services.AddSingleton(configureDb);

        MongoDbPersistence<ConfigurationDbMap>.Configure();
        services.AddScoped<IMongoContext>(x => new MongoContext(configureDb.ConnectionStringMongoDb, configureDb.DatabaseName));

        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void UseValidators(this IServiceCollection services)
    {
        services.AddScoped<IUserValidation, UserValidation>();
    }
}