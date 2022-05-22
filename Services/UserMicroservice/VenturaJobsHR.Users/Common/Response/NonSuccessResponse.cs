namespace VenturaJobsHR.Users.Common.Response;

public class NonSuccessResponse
{
    /// <summary>
    /// Http status of response
    /// </summary>
    [JsonProperty("statusCode", Order = 0)]
    public int StatusCode { get; set; }

    /// <summary>
    /// Message of response
    /// </summary>
    [JsonProperty("message", Order = 1)]
    public string Message { get; set; }
}
