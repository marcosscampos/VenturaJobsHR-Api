namespace VenturaJobsHR.Common.Exceptions;

[Serializable]
public class InvalidParameterException : Exception
{
    public InvalidParameterException(string message) : base(message)
    {

    }
}
