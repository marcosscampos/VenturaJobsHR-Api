using VenturaJobsHR.CrossCutting.Enums;
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

    protected virtual void NotifyNullOrEmptyObject()
    {
        Notification.RaiseError(CommonsEnum.Error.NullOrEmptyObject.ToString());
    }

    protected bool IsValid(BaseCommand command)
    {
        if (command.IsValid()) return true;

        foreach (var error in command.ValidationResult.Errors)
        {
            var commandError = error.CustomState as CommandErrorObject;
            Notification.RaiseError(commandError.Enum.ToString(), commandError.Reference);
        }

        return false;
    }

    protected IEnumerable<string> GetValidatedCodes<C>(C request, params string[] codes) where C : BaseCommand
    {
        codes = codes.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        if (!IsValid(request))
        {
            var notifications = Notification.GetNotifications();
            if (notifications.Any(x => x.Key.Equals(
                 CommonCommandError.DuplicatedItems.ToString()) ||
                 x.Key.Equals(CommonCommandError.MaxItemsExceeded.ToString()))) return null;

            var result = Notification.CheckErrorNotifications(codes.ToList());
            if (result.Values.All(x => x)) return null;

            return result.Where(x => !x.Value).Select(x => x.Key);
        }

        return codes;
    }

    protected IEnumerable<string> GetValidatedCodes<C>(C request, IEnumerable<string> codes) where C : BaseCommand
    {
        return GetValidatedCodes(request, codes.ToArray());
    }
}
