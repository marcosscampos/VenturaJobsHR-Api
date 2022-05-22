namespace VenturaJobsHR.Users.Common.Exceptions;

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