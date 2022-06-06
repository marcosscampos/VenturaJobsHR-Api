namespace VenturaJobsHR.Bff.CrossCutting.Http.Exceptions;

public class UnauthorizedException : Exception
{
    public readonly IDictionary<string, string> Errors;
    public UnauthorizedException(string message) : base(message)
    {
        Errors = new Dictionary<string, string>
        {
            { "UnauthorizedException", message }
        };
    }
}