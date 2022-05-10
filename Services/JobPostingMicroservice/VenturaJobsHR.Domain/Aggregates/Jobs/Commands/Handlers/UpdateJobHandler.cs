using MediatR;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class UpdateJobHandler : BaseJobHandler, IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;

    public UpdateJobHandler(INotificationHandler notification, IJobRepository jobRepository, IMediator mediator) : base(notification, mediator, jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request))
            return Unit.Value;

        foreach (var item in request.JobList)
        {
            var returnedJob = await _jobRepository.GetByIdAsync(item.Id);
            var updatedJob = request.EntityList.FirstOrDefault(x => x.Id == item.Id);

            if (updatedJob == null) continue;

            UpdateJob(item, updatedJob);

            if (!Notification.HasErrorNotifications(updatedJob.Id))
            {
                Notification.RaiseSuccess(updatedJob.Id, updatedJob.Description);
                await UpdateJob(updatedJob);
            }
        }

        return Unit.Value;
    }


    private void UpdateJob(CreateOrUpdateJobRequest request, Job job)
    {
        var salary = new Salary(request.Salary.Value);
        var location = new Location(request.Location.City, request.Location.State, request.Location.Country);
        var company = new Company(request.Company.Id, request.Company.Uid, request.Company.Name);

        job.Update(
            request.Id,
            request.Name,
            request.Description,
            salary,
            location,
            company,
            request.Status,
            request.FinalDate
            );

        if (request.CriteriaList.Any())
        {
            foreach (var item in request.CriteriaList)
            {
                var criteria = new Criteria(item.Name, item.Description, item.Profiletype, item.Weight);
                job.AddCriteria(criteria);
            }
        }
    }
}
