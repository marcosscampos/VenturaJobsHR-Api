using Newtonsoft.Json;

namespace VenturaJobsHR.Api.Common.Responses;

public class UnauthorizedResponse : NonSuccessResponse
{
    public UnauthorizedResponse(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    [JsonProperty("errors", Order = 2)]
    public IDictionary<string, string> Errors { get; }
}

