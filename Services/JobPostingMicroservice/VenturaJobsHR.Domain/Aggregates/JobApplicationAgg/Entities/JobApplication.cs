using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Entities;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;

public class JobApplication
{
    public JobApplication(string userId, string jobId, string id)
    {
        UserId = userId;
        JobId = jobId;
        CriteriaList = new List<JobCriteria>();
        CreatedAt = new DateTimeWithZone(DateTime.Now).LocalTime;
        Id = id;
    }

    public string Id { get; set; }
    public string UserId { get; set; }
    public string JobId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<JobCriteria> CriteriaList { get; set; }

    public static ProfileTypeEnum GetProfileTypeBy(int pt) => pt switch
    {
        1 => ProfileTypeEnum.Desirable,
        2 => ProfileTypeEnum.Differential,
        3 => ProfileTypeEnum.Relevant,
        4 => ProfileTypeEnum.VeryRelevant,
        5 => ProfileTypeEnum.Mandatory,
        _ => ProfileTypeEnum.Desirable
    };

    public void AddCriteria(JobCriteria criteria)
        => CriteriaList.Add(criteria);

    public static void JobApplicationDuplicated(INotificationHandler notification, string reference)
        => notification.RaiseError(EntityError.JobApplicationDuplicated, reference);

    public string GetKeyCache()
    => $"JOBAPPLICATION-#{UserId.ToUpper()}#{JobId.ToUpper()}";
}