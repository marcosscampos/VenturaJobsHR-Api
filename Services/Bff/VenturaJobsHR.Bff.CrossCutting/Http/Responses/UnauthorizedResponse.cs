using Newtonsoft.Json;

namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses;

public class UnauthorizedResponse : NonSuccessResponse
{
    public UnauthorizedResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}

