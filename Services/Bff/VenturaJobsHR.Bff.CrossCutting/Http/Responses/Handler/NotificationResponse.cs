namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses.Handler;

public class NotificationResponse
{
    public string Code { get; set; }
    public string Reference { get; set; }
    public List<NotificationResponseItem> Notifications { get; set; }
}

public class NotificationResponseItem
{
    public string Key { get; set; }
    public string Message { get; set; }
}