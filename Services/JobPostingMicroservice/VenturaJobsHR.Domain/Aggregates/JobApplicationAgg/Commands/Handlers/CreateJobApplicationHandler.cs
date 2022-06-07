using MediatR;
using MongoDB.Bson;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands.Handlers;

public class CreateJobApplicationHandler : BaseJobApplicationHandler, IRequestHandler<CreateJobApplicationCommand, Unit>
{
    public CreateJobApplicationHandler(INotificationHandler notification,
        IMediator mediator, 
        ICacheService cacheService, 
        IJobApplicationRepository applicationRepository,
        IJobRepository jobRepository) : base(notification, mediator, cacheService, applicationRepository, jobRepository)
    {
    }

    public async Task<Unit> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request)) return Unit.Value;

        if(await IsDuplicated(request))
        {
            JobApplication.JobApplicationDuplicated(Notification, request.Application.GetReference());
            return Unit.Value;
        }

        if(request != null && !Notification.HasErrorNotifications(request.Application.GetReference()))
        {
            Notification.RaiseSuccess(request.Application.UserId, request.Application.GetReference());

            var application = Create(request);
            await CreateApplicationAsync(application);
            await SetCache(application);
        }

        return Unit.Value;
    }

    private JobApplication Create(CreateJobApplicationCommand request)
    {
        var id = ObjectId.GenerateNewId().ToString();
        var applicationRequest = request.Application;
        var application = new JobApplication(applicationRequest.UserId, applicationRequest.JobId, id);

        foreach(var item in applicationRequest.CriteriaList)
        {
            application.AddCriteria(new JobCriteria(item.CriteriaId, item.ProfileType));
        }

        return application;
    }
}
