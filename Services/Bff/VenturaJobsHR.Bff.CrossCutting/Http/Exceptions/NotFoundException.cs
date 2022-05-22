namespace VenturaJobsHR.Bff.CrossCutting.Http.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public readonly IDictionary<string, string> Errors;
    public NotFoundException(string message) : base(message)
    {
        Errors = new Dictionary<string, string>
        {
            { "NotFoundException", message }
        };
    }
}
