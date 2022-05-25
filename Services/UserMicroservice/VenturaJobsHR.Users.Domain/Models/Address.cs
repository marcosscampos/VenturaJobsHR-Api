namespace VenturaJobsHR.Users.Domain.Models;

public class Address
{
    public Address(string completeAddress, string complement, string postalCode, string city, string state)
    {
        CompleteAddress = completeAddress;
        Complement = complement;
        PostalCode = postalCode;
        City = city;
        State = state;
    }

    public Address()
    {

    }

    public string CompleteAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Complement { get; set; }
    public string PostalCode { get; set; }
}
