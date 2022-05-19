using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VenturaJobsHR.Users.Common.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ApiExceptionMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var error = ErrorHandlerFactory.Create(ex);
        return CreateResponse(context, error);
    }

    private static Task CreateResponse(HttpContext context, ErrorHandler error)
    {
        context.Response.ContentType = "application/json";        

        var result = JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(result);
    }
}
