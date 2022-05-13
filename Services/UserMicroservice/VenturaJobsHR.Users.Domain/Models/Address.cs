namespace VenturaJobsHR.Users.Domain.Models;

public class Address
{
    public Address(string completeAddress, string district, string complement, string postalCode, string number)
    {
        CompleteAddress = completeAddress;
        District = district;
        Complement = complement;
        PostalCode = postalCode;
        Number = number;
    }

    public Address()
    {

    }

    public string CompleteAddress { get; set; }
    public string District { get; set; }
    public string Complement { get; set; }
    public string PostalCode { get; set; }
    public string Number { get; set; }
}
