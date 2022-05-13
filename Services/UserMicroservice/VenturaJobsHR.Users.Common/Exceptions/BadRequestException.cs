using System.Runtime.Serialization;

namespace VenturaJobsHR.Users.Common.Exceptions;

[Serializable]
public class BadRequestException : Exception
{
    private IDictionary<string, string> _Errors;
    public IDictionary<string, string> Errors { get => _Errors; set => _Errors = value; }

    public BadRequestException(IDictionary<string, string> errors)
    {
        Errors = errors;
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
