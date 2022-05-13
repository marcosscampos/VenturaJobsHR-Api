namespace VenturaJobsHR.Users.Domain.Models;

public class LegalRecord
{
    public LegalRecord(string? corporateName, string? cpf, string? cnpj)
    {
        CorporateName = corporateName;
        CPF = cpf;
        CNPJ = cnpj;
    }

    public LegalRecord()
    {

    }

    public string? CorporateName { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
}
