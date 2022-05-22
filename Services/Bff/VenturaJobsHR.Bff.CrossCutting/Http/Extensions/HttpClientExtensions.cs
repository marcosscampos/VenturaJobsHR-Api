using Newtonsoft.Json;
using System.Net;
using System.Text;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;

namespace VenturaJobsHR.Bff.CrossCutting.Http.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse> GetAsync<TResponse>(this HttpClient httpClient, string endpoint)
    {
        var response = await httpClient.GetAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return default;

        return await response.Content.ToResponseTypeAsync<TResponse>();
    }

    public static async Task<CreatedResponse<object>> GetSingleAsync<TResponse>(this HttpClient httpClient, string endpoint)
    {
        var response = await httpClient.GetAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return new CreatedResponse<object>("", (await response.Content.ToResponseTypeAsync<BadRequestResponse>()).Errors);

        return new CreatedResponse<object>(await response.Content.ToResponseTypeAsync<object>());
    }

    public static async Task<CreatedResponse<object>> PostAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var response = await httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return new CreatedResponse<object>(payload, (await response.Content.ToResponseTypeAsync<BadRequestResponse>()).Errors);

        response.EnsureSuccessStatusCode();

        return new CreatedResponse<object>(await response.Content.ToResponseTypeAsync<object>());
    }

    public static async Task<CreatedResponse<object>> PutAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var response = await httpClient.PutAsync(endpoint, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return new CreatedResponse<object>(payload, (await response.Content.ToResponseTypeAsync<BadRequestResponse>()).Errors);

        response.EnsureSuccessStatusCode();

        return new CreatedResponse<object>(response.Content.ReadAsStringAsync().Result);
    }

    public static async Task<CreatedResponse<object>> RemoveAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var response = await httpClient.DeleteAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return new CreatedResponse<object>(payload, (await response.Content.ToResponseTypeAsync<BadRequestResponse>()).Errors);

        response.EnsureSuccessStatusCode();

        return new CreatedResponse<object>(response.Content.ReadAsStringAsync().Result);
    }

    public static async Task<CreatedResponse<object>> PatchAsync<TPayload>(this HttpClient httpClient, string endpoint, TPayload payload)
    {
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };

        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            return new CreatedResponse<object>(payload, (await response.Content.ToResponseTypeAsync<BadRequestResponse>()).Errors);

        response.EnsureSuccessStatusCode();

        return new CreatedResponse<object>(response.Content.ReadAsStringAsync().Result);
    }

    public static async Task<TResponse> ToResponseTypeAsync<TResponse>(this HttpContent httpContent)
        => JsonConvert.DeserializeObject<TResponse>(await httpContent.ReadAsStringAsync());

    public static TResponse ToResponseType<TResponse>(this HttpContent httpContent)
        => JsonConvert.DeserializeObject<TResponse>(httpContent.ReadAsStringAsync().Result);
}
