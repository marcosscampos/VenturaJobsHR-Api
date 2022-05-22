using System.Net;
using System.Text.Json;
using VenturaJobsHR.Api.Common.Responses;
using VenturaJobsHR.Common.Exceptions;

namespace VenturaJobsHR.Api.Common.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(ILogger<ApiExceptionMiddleware> logger, RequestDelegate request)
    {
        _logger = logger;
        _next = request;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.BadRequest;

        switch (exception)
        {
            case NotFoundException ex:
                var notFound = new NotFoundResponse(ex.Errors)
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "An error occurred while processing your request."
                };

                response.StatusCode = (int)HttpStatusCode.NotFound;
                await response.WriteAsync(JsonSerializer.Serialize(notFound));
                break;
            case InvalidEntityIdProvidedException ex:
                var invalidEntity = new BadRequestResponse(ex.Errors)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "An error occurred while processing your request."
                };

                await response.WriteAsync(JsonSerializer.Serialize(invalidEntity));
                break;
            case InvalidParameterException ex:
                var invalidParameter = new BadRequestResponse(ex.Errors)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "An error occurred while processing your request."
                };

                await response.WriteAsync(JsonSerializer.Serialize(invalidParameter));
                break;
            case BadRequestException ex:
                var badRequest = new BadRequestResponse(ex.Errors)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Failed to validate parameters."
                };

                await response.WriteAsync(JsonSerializer.Serialize(badRequest));
                break;
            default:
                var nonSuccess = new NonSuccessResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = exception.Message
                };

                await response.WriteAsync(JsonSerializer.Serialize(nonSuccess));
                break;
        }
    }
}
