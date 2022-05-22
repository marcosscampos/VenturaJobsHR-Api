namespace VenturaJobsHR.Users.Common.Response;

public class BadRequestResponse : NonSuccessResponse
{
    public BadRequestResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}
