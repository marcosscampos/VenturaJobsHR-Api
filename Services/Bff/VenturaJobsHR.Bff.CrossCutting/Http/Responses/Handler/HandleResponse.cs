namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses.Handler;

public class HandleResponse
{
    public List<NotificationResponse> Errors { get; set; }
    public List<NotificationResponse> Success { get; set; }
    public object Data { get; set; }
}
