using System.Runtime.Serialization;

namespace VenturaJobsHR.Users.Common.Exceptions;

[Serializable]
public class BadRequestException : Exception
{
    public IDictionary<string, string> Errors { get; set; }

    public BadRequestException(IDictionary<string, string> errors)
    {
        Errors = errors;
    }
}
