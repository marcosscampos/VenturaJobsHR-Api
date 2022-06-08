using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.UserAgg.Entities;

public class User
{
    public string Id { get; set; }
    public string FirebaseId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public UserTypeEnum UserType { get; set; }
    public bool Active { get; set; }
    
    public static string GetUserTypeBy(UserTypeEnum user) => user switch
    {
        UserTypeEnum.Company => "company",
        UserTypeEnum.Applicant => "applicant",
        UserTypeEnum.Admin => "admin",
        _ => string.Empty
    };
}
