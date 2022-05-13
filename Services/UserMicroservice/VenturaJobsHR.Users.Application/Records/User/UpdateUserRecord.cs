using VenturaJobsHR.Users.Application.Records.User.Miscellaneous;

namespace VenturaJobsHR.Users.Application.Records.User;

public record UpdateUserRecord(string Id, string Name, string Phone, string Email, AddressRecord Address, SocialRegisterRecord LegalRecord);