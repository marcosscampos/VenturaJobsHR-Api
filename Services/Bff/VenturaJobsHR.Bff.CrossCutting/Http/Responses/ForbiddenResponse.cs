using Newtonsoft.Json;

namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses;

public class ForbiddenResponse : NonSuccessResponse
{
    public ForbiddenResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}