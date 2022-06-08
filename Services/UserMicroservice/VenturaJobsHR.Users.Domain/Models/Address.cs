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

    public void Update(string completeAddress, string complement, string postalCode, string city, string state)
    {
        if (!CompleteAddress.Equals(completeAddress))
            CompleteAddress = completeAddress;

        if (!Complement.Equals(complement))
            Complement = complement;

        if (!PostalCode.Equals(postalCode))
            PostalCode = postalCode;

        if (!City.Equals(city))
            City = city;

        if (!State.Equals(state))
            State = state;
    }

    public string CompleteAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Complement { get; set; }
    public string PostalCode { get; set; }
}
