namespace VenturaJobsHR.Users.Common.Response;

public class ForbiddenResponse : NonSuccessResponse
{
    public ForbiddenResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}