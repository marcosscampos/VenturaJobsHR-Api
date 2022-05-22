using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Domain.Models.Users;

public class User
{
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
