namespace VenturaJobsHR.Message.Common;

public class ErrorHandlerMessage<T>
{
    public List<string> Errors { get; set; }
    public T MessageObject { get; set; }
}
