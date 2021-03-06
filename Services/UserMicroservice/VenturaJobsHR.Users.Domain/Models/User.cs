using VenturaJobsHR.Users.CrossCutting.Enums;

namespace VenturaJobsHR.Users.Domain.Models;

public class User
{
    public User()
    {
    }

    public User(string firebaseId, string name, string phone, string email, Address address, UserTypeEnum userType,
        LegalRecord legalRecord, bool active = true)
    {
        Id = string.Empty;
        FirebaseId = firebaseId;
        Name = name;
        Phone = phone;
        Email = email;
        Address = address;
        UserType = userType;
        LegalRecord = legalRecord;
        Active = active;
    }

    public void Update(
        string id,
        string name,
        string phone,
        string email,
        Address address,
        LegalRecord legalRecord)
    {
        Id = id;

        if (!Name.Equals(name))
            Name = name;

        if (!Phone.Equals(phone))
            Phone = phone;

        if (!Email.Equals(email))
            Email = email;

        Address = address;
        LegalRecord = legalRecord;
    }

    public static string GetUserTypeBy(UserTypeEnum user) => user switch
    {
        UserTypeEnum.Company => "company",
        UserTypeEnum.Applicant => "applicant",
        UserTypeEnum.Admin => "admin",
        _ => string.Empty
    };


    public string Id { get; set; }
    public string FirebaseId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public UserTypeEnum UserType { get; set; }
    public LegalRecord LegalRecord { get; set; }
    public bool Active { get; set; }
}