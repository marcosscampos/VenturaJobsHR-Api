using VenturaJobsHR.Users.Application.Records.User.Miscellaneous;
using VenturaJobsHR.Users.CrossCutting.Enums;

namespace VenturaJobsHR.Users.Application.Records.User;

public record CreateUserRecord(
    string FirebaseId, 
    string Name, 
    string Phone, 
    string Email, 
    AddressRecord Address, 
    UserTypeEnum UserType, 
    SocialRegisterRecord LegalRecord);