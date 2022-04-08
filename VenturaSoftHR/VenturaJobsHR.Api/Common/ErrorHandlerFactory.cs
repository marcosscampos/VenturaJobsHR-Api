using VenturaJobsHR.Common.Exceptions;

namespace VenturaJobsHR.Api.Common;

public static class ErrorHandlerFactory
{
    public static ErrorHandler Create(Exception exception) => exception switch
    {
        InvalidParameterException e => new ErrorHandler(e.GetType().Name, e.Message),
        GenericErrorException e => new ErrorHandler(e.GetType().Name, e.Message),
        NotFoundException e => new ErrorHandler(e.GetType().Name, e.Message),
        _ => new ErrorHandler(exception.GetType().Name, exception.Message)
    };
}
