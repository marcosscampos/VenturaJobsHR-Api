using MediatR;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Factories;
using VenturaJobsHR.Domain.Aggregates.Jobs.Repositories;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Handlers;

public class UpdateJobHandler : BaseJobHandler, IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IJobRepository _jobRepository;

    public UpdateJobHandler(INotificationHandler notification, IJobRepository jobRepository, IMediator mediator) : base(notification, mediator)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        if (!IsValid(request))
            return Unit.Value;

        if (!Notification.HasErrorNotifications())
        {
            foreach (var items in request.Job)
            {
                Notification.RaiseSuccess(items.Id, items.Name);

                var job = await _jobRepository.GetByIdAsync(items.Id);

                var UpdatedJob = JobFactory.Create(items.Name, items.Description, items.Salary.Value, items.FinalDate);
                job.Name = UpdatedJob.Name ?? job.Name;
                job.Description = UpdatedJob.Description ?? job.Description;
                job.Salary = UpdatedJob.Salary ?? job.Salary;
                job.FinalDate = UpdatedJob.FinalDate != DateTime.MinValue ? UpdatedJob.FinalDate : job.FinalDate;

                await _jobRepository.UpdateAsync(job);
                await UpdateJob(job);
            }
        }

        return Unit.Value;
    }
}
