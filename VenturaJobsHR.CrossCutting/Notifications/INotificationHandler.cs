namespace VenturaJobsHR.CrossCutting.Notifications;

public interface INotificationHandler
{
    bool HasNotifications();
    bool HasErrorNotifications(string reference = null);
    List<Notification> GetNotifications();
    void RaiseError(string key, string reference = null);
    void RaiseError(Enum en, string reference = null);
    void RaiseSuccess(string referenceId, string reference);
    void RaiseWarning(string key, string reference = null);
    Dictionary<string, bool> CheckErrorNotifications(List<string> referenceList);
    void RaiseInformation(string key, string value, string reference = null);
    void RaiseErrorMessage(string message);
    List<string> GetTraceNotifications();
    void RaiseTrace(string message);
}
