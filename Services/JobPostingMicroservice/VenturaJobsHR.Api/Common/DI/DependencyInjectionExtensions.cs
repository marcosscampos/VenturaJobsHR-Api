using System.Reflection;
using Coravel;
using MediatR;
using StackExchange.Redis;
using VenturaJobsHR.Api.Common.Jobs;
using VenturaJobsHR.Application.Services.Concretes;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.Common.Mapping;
using VenturaJobsHR.CrossCutting.Localizations;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.Common.Settings;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;
using VenturaJobsHR.Repository;
using VenturaJobsHR.Repository.Context;
using VenturaJobsHR.Repository.DatabaseSettings;
using VenturaJobsHR.Repository.Mappings;
using VenturaJobsHR.Repository.Persistence;

namespace VenturaJobsHR.Api.Common.DI;

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
        services.ConfigureRedis(configuration);

        return services;
    }

    private static void UseServices(this IServiceCollection services)
    {
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();

        services.AddScoped<INotificationHandler, NotificationHandler>();
        services.AddScoped<ILocalizationManager, LocalizationManager>();
        MapperFactory.Setup();

        services.AddMediatR(typeof(CreateJobCommand).GetTypeInfo().Assembly);
        services.AddMediatR(typeof(UpdateJobCommand).GetTypeInfo().Assembly);
        services.AddScheduler();

        services.AddTransient<Worker>();
    }

    private static void UseRepositories(this IServiceCollection services, Action<IDbSettings> dbSettings)
    {
        IDbSettings configureDb = new DbSettings();
        dbSettings.Invoke(configureDb);
        services.AddSingleton(configureDb);

        MongoDbPersistence<ConfigurationDbMap>.Configure();
        services.AddScoped<IMongoContext>(x => new MongoContext(configureDb.ConnectionStringMongoDb, configureDb.DatabaseName));

        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void ConfigureRedis(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDistributedRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("Redis:Uri");
            opt.ConfigurationOptions = new ConfigurationOptions()
            {
                Password = configuration["ConnectionStrings:Redis:Auth"],
                AbortOnConnectFail = true,
                Ssl = true,
                EndPoints = { configuration.GetConnectionString("Redis:Uri") },
                ConnectTimeout = 5000,
                SyncTimeout = 5000
            };
            opt.InstanceName = "VenturaJobsCache";
        });
    }
    
    
}
