namespace VenturaJobsHR.Users.Common.Response;

public class NotFoundResponse : NonSuccessResponse
{

    public NotFoundResponse()
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }

    public NotFoundResponse(string name, string message)
    {
        Errors = new Dictionary<string, string>
            {
                { name, message }
            };
    }

    public NotFoundResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}
