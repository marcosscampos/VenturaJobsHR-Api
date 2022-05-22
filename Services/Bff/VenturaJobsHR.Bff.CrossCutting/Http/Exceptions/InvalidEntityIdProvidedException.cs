namespace VenturaJobsHR.Bff.CrossCutting.Http.Exceptions;

[Serializable]
public class InvalidEntityIdProvidedException : Exception
{
    public readonly IDictionary<string, string> Errors;
    public InvalidEntityIdProvidedException(string message) : base(message)
    {
        Errors = new Dictionary<string, string>
        {
            { "InvalidEntityIdProvidedException", message }
        };
    }
}