namespace VenturaJobsHR.Domain.SeedWork.Commands;

public class CommandErrorObject
{
    public Enum Enum { get; private set; }
    public string Reference { get; private set; }

    public CommandErrorObject(Enum enumeration, string reference = null)
    {
        Enum = enumeration;
        Reference = reference;
    }
}
