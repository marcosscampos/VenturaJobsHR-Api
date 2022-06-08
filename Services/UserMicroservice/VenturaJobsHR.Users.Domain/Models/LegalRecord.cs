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

    public void Update(string? corporateName, string? cpf, string? cnpj)
    {
        if (CorporateName != null && !CorporateName.Equals(corporateName))
            CorporateName = corporateName;

        if (CPF != null && !CPF.Equals(cpf))
            CPF = cpf;

        if (CNPJ != null && !CNPJ.Equals(cnpj))
            CNPJ = cnpj;
    }

    public string? CorporateName { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
}
