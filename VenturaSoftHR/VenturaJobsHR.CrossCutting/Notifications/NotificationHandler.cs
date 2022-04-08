using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Localizations;

namespace VenturaJobsHR.CrossCutting.Notifications;

public class NotificationHandler : INotificationHandler
{
    private List<Notification> _notifications;
    private readonly ILocalizationManager _localizationManager;
    public NotificationHandler(ILocalizationManager localizationManager)
    {
        _localizationManager = localizationManager;
        _notifications = new List<Notification>();
    }


    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public List<string> GetTraceNotifications()
        => _notifications.FindAll(x => x.Type == NotificationType.Trace).Select(x => x.Value).ToList();

    public bool HasErrorNotifications(string reference = null)
        => _notifications.Any(x => x.Type == NotificationType.Error && (string.IsNullOrWhiteSpace(reference) || x.Reference == reference));

    public Dictionary<string, bool> CheckErrorNotifications(List<string> referenceList)
    {
        var items = new Dictionary<string, bool>();
        foreach (var reference in referenceList)
        {
            if (!items.Any(x => x.Key?.Equals(reference) == true))
                items.Add(reference, HasErrorNotifications(reference));
        }

        return items;
    }

    public bool HasNotifications()
       => _notifications.Any();

    public void RaiseError(Enum en, string reference = null)
    {
        RaiseError(en.ToString(), reference);
    }

    public void RaiseError(string key, string reference = null)
    {
        if (!_notifications.Any(n => n.Reference == reference && key.Equals(n.Key)))
            _notifications.Add(new Notification(key, _localizationManager.GetValue(key), NotificationType.Error, reference));
    }

    public void RaiseErrorMessage(string message)
    {
        _notifications.Add(new Notification("GenericError", message, NotificationType.Error, Guid.NewGuid().ToString()));
    }

    public void RaiseWarning(string key, string reference = null)
    {
        if (!_notifications.Any(n => n.Reference == reference && key.Equals(n.Key)))
            _notifications.Add(new Notification(key, _localizationManager.GetValue(key), NotificationType.Warning, reference));

    }

    public void RaiseInformation(string key, string value, string reference = null)
    {
        if (!_notifications.Any(n => n.Reference == reference && key.Equals(n.Key)))
            _notifications.Add(new Notification(key, value, NotificationType.Information, reference));

    }

    public void RaiseSuccess(string referenceId, string reference)
    {
        var key = "EntityProcessed";
        if (!_notifications.Any(n => n.ReferenceId == referenceId))
            _notifications.Add(new Notification(key, _localizationManager.GetValue(key), NotificationType.Success, reference, referenceId));
    }

    public void RaiseTrace(string message)
      => _notifications.Add(new Notification(Guid.NewGuid().ToString(), message, NotificationType.Trace));

    public void Dispose()
        => _notifications = new List<Notification>();
}
