using VenturaJobsHR.CrossCutting.Notifications;

namespace VenturaJobsHR.CrossCutting.Responses;

public class HandleResponse
{
    public List<NotificationResponse> Errors { get; set; }
    public List<NotificationResponse> Success { get; set; }
    public object Data { get; set; }
}
