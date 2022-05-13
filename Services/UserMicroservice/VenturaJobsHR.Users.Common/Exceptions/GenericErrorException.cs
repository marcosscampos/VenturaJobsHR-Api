namespace VenturaJobsHR.Users.Common.Exceptions;

[Serializable]
public class GenericErrorException : Exception
{
    public GenericErrorException(string message) : base(message)
    {
    }
}