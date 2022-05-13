using Microsoft.AspNetCore.Mvc;

namespace VenturaJobsHR.Users.Common.Handler.Errors;

public static class ErrorResult
{
    public static ObjectResult ReturnErrorResult(Exception ex)
    {
        var error = ErrorHandlerFactory.Create(ex);

        if (error.ErrorName.Contains("NotFoundException"))
            return new NotFoundObjectResult(error);

        return new BadRequestObjectResult(error);
    }
}
