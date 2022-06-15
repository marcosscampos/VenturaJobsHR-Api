using System.Text.Json.Serialization;

namespace VenturaJobsHR.Bff.CrossCutting.Http.Responses.Handler;

public class HandleResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? StatusCode { get; set; }
    public IDictionary<string, string> Errors { get; set; }
}
