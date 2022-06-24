using System.Linq.Expressions;
using MediatR;
using Microsoft.AspNetCore.Http;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Concretes;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Handlers;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Events;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;
using VenturaJobsHR.Tests.Resources.Data;
using static VenturaJobsHR.Tests.Resources.Data.DataBuilder;

namespace VenturaJobsHR.Tests.Services;

public class JobServiceTest
{
    private readonly Mock<IJobRepository> _jobRepository;
    private readonly Mock<IJobApplicationRepository> _jobApplicationRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IHttpContextAccessor> _httpContext;
    private readonly Mock<INotificationHandler> _notificationHandlerMock;
    private readonly Mock<ICacheService> _cacheService;

    public JobServiceTest()
    {
        _jobRepository = new Mock<IJobRepository>();
        _jobApplicationRepository = new Mock<IJobApplicationRepository>();
        _userRepository = new Mock<IUserRepository>();
        _mediatorMock = new Mock<IMediator>();
        _notificationHandlerMock = new Mock<INotificationHandler>();
        _httpContext = new Mock<IHttpContextAccessor>();
        _cacheService = new Mock<ICacheService>();
    }

    private JobService InitializeServices()
        => new(_jobRepository.Object,
            _mediatorMock.Object,
            _notificationHandlerMock.Object,
            _httpContext.Object, 
            _jobApplicationRepository.Object,
            _userRepository.Object);

    [Fact]
    public async Task ShouldListAllJobs()
    {
        var service = InitializeServices();
        var query = new SearchJobsQuery();
        _jobRepository
            .Setup(x => x.FindByFilterAsync(It.IsAny<Expression<Func<Job, bool>>>(), It.IsAny<Page>()))
            .Returns(() => DataBuilder.GetAllJobs());

        var jobs = await service.GetAllJobsByCriteriaAndPaged(query);

        Assert.IsType<Pagination<GetJobsRecord>>(jobs);
        Assert.NotNull(jobs);
    }

    [Fact]
    public async Task ShouldCreateAJob()
    {
        var service = InitializeServices();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateJobCommand>(), new CancellationToken())).Returns(Task.FromResult(Unit.Value));
        _mediatorMock.Setup(x => x.Publish(It.IsAny<JobsCreatedEvent>(), new CancellationToken())).Returns(Task.FromResult(Unit.Value));
        _cacheService.Setup(x => x.InsertOrUpdateAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTime>())).Returns(Task.CompletedTask);
        _cacheService.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(Task.FromResult(false));
        _jobRepository.Setup(x => x.BulkInsertAsync(It.IsAny<List<Job>>())).Returns(Task.CompletedTask);

        var command = await DataBuilder.GetJobCommand();
        var isCreated = service.CreateJob(command).IsCompletedSuccessfully;

        Assert.True(isCreated);
    }
}