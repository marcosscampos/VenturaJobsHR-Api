namespace VenturaJobsHR.Bff.CrossCutting.Http.Exceptions;

public class ForbiddenException : Exception
{
    public readonly IDictionary<string, string> Errors;
    public ForbiddenException(string message) : base(message)
    {
        Errors = new Dictionary<string, string>
        {
            { "ForbiddenException", message }
        };
    }
}

