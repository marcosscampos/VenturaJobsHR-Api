namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses;

public class CreatedResponse<TResponse>
{
    public TResponse Content { get; set; }
    public IDictionary<string, string> Errors { get; set; }

    public CreatedResponse(TResponse content)
    {
        Content = content;
    }

    public CreatedResponse(TResponse content, IDictionary<string, string> errors)
    {
        Content = content;
        Errors = errors;
    }
}
