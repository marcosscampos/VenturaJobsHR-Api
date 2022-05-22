using VenturaJobsHR.Bff.Application.Records.User.Miscellaneous;

namespace VenturaJobsHR.Bff.Application.Records.User;

public record UpdateUserRecord(string Id, string Name, string Phone, string Email, AddressRecord Address, SocialRegisterRecord LegalRecord);