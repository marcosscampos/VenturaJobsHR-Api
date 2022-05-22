using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Domain.Factory;

public static class HttpClientFactory
{
    public static HttpClient GetClient(this IHttpClientFactory factory, HttpClientKeysEnum httpClientKeys)
        => factory.CreateClient(httpClientKeys.ToString());
}
