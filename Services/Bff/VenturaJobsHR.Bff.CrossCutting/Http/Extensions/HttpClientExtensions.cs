using Newtonsoft.Json;
using System.Net;
using System.Text;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;

namespace VenturaJobsHR.Bff.CrossCutting.Http.Extensions;

public static class HttpClientExtensions
{
    public static async Task<object> GetAsync<TResponse>(this HttpClient httpClient, string endpoint)
    {
        var response = await httpClient.GetAsync(endpoint);

        return await ToResponse<object>(response);
    }

    public static async Task<object> PostAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        return await ToResponse<object>(response);
    }

    public static async Task<object> PutAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var response = await httpClient.PutAsync(endpoint, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        return await ToResponse<object>(response);
    }

    public static async Task<object> RemoveAsync<TPayload>(this HttpClient httpClient, string endpoint)
    {
        var response = await httpClient.DeleteAsync(endpoint);

        return await ToResponse<object>(response);
    }

    public static async Task<object> PatchAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };

        var response = await httpClient.SendAsync(request);

        return await ToResponse<object>(response);
    }

    public static async Task<TResponse> ToResponseTypeAsync<TResponse>(this HttpContent httpContent)
        => JsonConvert.DeserializeObject<TResponse>(await httpContent.ReadAsStringAsync());

    public static TResponse ToResponseType<TResponse>(this HttpContent httpContent)
        => JsonConvert.DeserializeObject<TResponse>(httpContent.ReadAsStringAsync().Result);

    private static async Task<object> ToResponse<TResponse>(HttpResponseMessage response)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                return await response.Content.ToResponseTypeAsync<BadRequestResponse>();

            case HttpStatusCode.NotFound:
                return await response.Content.ToResponseTypeAsync<NotFoundResponse>();

            case HttpStatusCode.Forbidden:
                return await response.Content.ToResponseTypeAsync<ForbiddenResponse>();

            case HttpStatusCode.Unauthorized:
                return await response.Content.ToResponseTypeAsync<UnauthorizedResponse>();

            case HttpStatusCode.OK:
            case HttpStatusCode.Created:
                response.EnsureSuccessStatusCode();
                return await response.Content.ToResponseTypeAsync<object>();

            default:
                return await response.Content.ToResponseTypeAsync<BadRequestResponse>();
        }
    }
}
