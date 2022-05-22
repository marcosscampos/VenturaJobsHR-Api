namespace VenturaJobsHR.Common.Exceptions;

[Serializable]
public class InvalidParameterException : Exception
{
    public readonly IDictionary<string, string> Errors;
    public InvalidParameterException(string message) : base(message)
    {
        Errors = new Dictionary<string, string>
        {
            { "InvalidParameterException", message }
        };
    }
}