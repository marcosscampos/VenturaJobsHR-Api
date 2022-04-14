using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.CrossCutting.Notifications;

public class Notification
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    public NotificationType Type { get; private set; }
    public string ReferenceId { get; set; }
    public string Reference { get; private set; }

    public Notification(string key, string value, NotificationType type, string reference = null, string referenceId = null)
    {
        Key = key;
        Value = value;
        Reference = reference;
        Type = type;
        ReferenceId = referenceId;
    }
}
