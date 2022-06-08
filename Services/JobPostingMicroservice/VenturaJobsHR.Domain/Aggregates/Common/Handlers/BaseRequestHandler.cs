using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;

namespace VenturaJobsHR.Domain.Aggregates.Common.Handlers;

public class BaseRequestHandler
{
    protected readonly INotificationHandler Notification;

    protected BaseRequestHandler(INotificationHandler notification)
    {
        Notification = notification;
    }

    protected bool IsValid(BaseCommand command)
    {
        if (command.IsValid()) return true;

        foreach (var commandError in command.ValidationResult.Errors.Select(error =>
                     error.CustomState as CommandErrorObject))
        {
            Notification.RaiseError(commandError.Enum.ToString(), commandError.Reference);
        }

        return false;
    }
}