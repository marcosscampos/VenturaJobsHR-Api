using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VenturaJobsHR.Users.Application.Services.Concrete;
using VenturaJobsHR.Users.Application.Services.Interface;
using VenturaJobsHR.Users.Common.Mappings;
using VenturaJobsHR.Users.Domain.Abstractions.Repositories;
using VenturaJobsHR.Users.Domain.Seedwork.Settings;
using VenturaJobsHR.Users.Repository;
using VenturaJobsHR.Users.Repository.Context;
using VenturaJobsHR.Users.Repository.DatabaseSettings;
using VenturaJobsHR.Users.Repository.Mappings;
using VenturaJobsHR.Users.Repository.Persistence;

namespace VenturaJobsHR.Users.Application.DI;

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
        return services;
    }

    private static void UseServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        //services.AddTransient<IBusService, BusService>();

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
}