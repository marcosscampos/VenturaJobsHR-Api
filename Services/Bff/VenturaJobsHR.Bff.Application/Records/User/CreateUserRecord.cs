using VenturaJobsHR.Bff.Application.Records.User.Miscellaneous;
using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.User;

public record CreateUserRecord(
    string FirebaseId,
    string Name,
    string Phone,
    string Email,
    AddressRecord Address,
    UserTypeEnum UserType,
    SocialRegisterRecord LegalRecord);
