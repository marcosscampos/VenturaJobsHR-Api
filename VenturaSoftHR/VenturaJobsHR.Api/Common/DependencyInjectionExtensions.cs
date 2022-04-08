using MediatR;
using System.Reflection;
using VenturaJobsHR.Application.Services.Concretes;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.Common.Mapping;
using VenturaJobsHR.CrossCutting.Localizations;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;
using VenturaJobsHR.Domain.SeedWork.Settings;
using VenturaJobsHR.Repository;
using VenturaJobsHR.Repository.Context;
using VenturaJobsHR.Repository.DatabaseSettings;
using VenturaJobsHR.Repository.Mappings;
using VenturaJobsHR.Repository.Persistence;

namespace VenturaJobsHR.Api.Common;

public static class DependencyInjectionExtensions
{
    public static void ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseRepositories(dbSettings =>
        {
            dbSettings.ConnectionStringMongoDb = configuration["ConnectionStrings:MongoDb:Uri"];
            dbSettings.DatabaseName = configuration["ConnectionStrings:MongoDb:DatabaseName"];
        });
        services.USeServices();
    }

    private static void USeServices(this IServiceCollection services)
    {
        services.AddScoped<IJobService, JobService>();


        services.AddScoped<INotificationHandler, NotificationHandler>();
        services.AddScoped<ILocalizationManager, LocalizationManager>();
        services.AddMediatR(typeof(CreateJobCommand).GetTypeInfo().Assembly);
        MapperFactory.Setup();
    }

    private static void UseRepositories(this IServiceCollection services, Action<IDbSettings> dbSettings)
    {
        IDbSettings configureDb = new DbSettings();
        dbSettings.Invoke(configureDb);
        services.AddSingleton(configureDb);

        MongoDbPersistence<ConfigurationDbMap>.Configure();
        services.AddScoped<IMongoContext>(x => new MongoContext(configureDb.ConnectionStringMongoDb, configureDb.DatabaseName));

        services.AddScoped<IJobRepository, JobRepository>();
    }
}
