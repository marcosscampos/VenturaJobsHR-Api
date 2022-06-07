using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Common.Entities;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

public class Job : Entity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Salary Salary { get; set; }
    public Location Location { get; set; }
    public Company Company { get; set; }
    public FormOfHiringEnum FormOfHiring { get; set; }
    public OccupationAreaEnum OccupationArea { get; set; }
    public List<Criteria> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime DeadLine { get; set; }
    public bool Active { get; set; }

    public Job(string name, string description, Salary salary, Location location, Company company, FormOfHiringEnum formOfHiring, OccupationAreaEnum occupationArea, JobStatusEnum status, DateTime deadLine)
    {
        Name = name;
        Description = description;
        Salary = salary;
        Location = location;
        Company = company;
        FormOfHiring = formOfHiring;
        OccupationArea = occupationArea;
        CriteriaList = new List<Criteria>();
        Status = status;
        DeadLine = deadLine;
        CreationDate = new DateTimeWithZone(DateTime.Now).LocalTime;
        Active = true;
    }

    public Job(string id, string name, string description, Salary salary, Location location, Company company, FormOfHiringEnum formOfHiring, OccupationAreaEnum occupationArea, JobStatusEnum status, DateTime deadLine)
        : this(name, description, salary, location, company, formOfHiring, occupationArea, status, deadLine)
    {
        Id = id;
    }

    public void Update
        (
            string id,
            string name,
            string description,
            Salary salary,
            Location location,
            Company company,
            JobStatusEnum status,
            OccupationAreaEnum occupationArea,
            FormOfHiringEnum formOfHiring,
            DateTime deadLine
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Salary = salary;
        Location = location;
        Company = company;
        FormOfHiring = formOfHiring;
        OccupationArea = occupationArea;
        Status = status;
        DeadLine = deadLine;
        LastUpdate = new DateTimeWithZone(DateTime.Now).LocalTime;
    }

    public static int GetProfileTypeBy(ProfileTypeEnum pt) => pt switch
    {
        ProfileTypeEnum.Desirable => 1,
        ProfileTypeEnum.Differential => 2,
        ProfileTypeEnum.Relevant => 3,
        ProfileTypeEnum.VeryRelevant => 4,
        ProfileTypeEnum.Mandatory => 5,
        _ => 1
    };

    public static OccupationAreaEnum GetOccupationAreaBy(int occupation) => occupation switch
    {
        1 => OccupationAreaEnum.Management,
        2 => OccupationAreaEnum.Infrastructure,
        3 => OccupationAreaEnum.Development,
        4 => OccupationAreaEnum.Database,
        5 => OccupationAreaEnum.Security,
        6 => OccupationAreaEnum.Design,
        _ => OccupationAreaEnum.Development
    };

    public Job() { }

    public void AddCriteria(Criteria criteria)
        => CriteriaList.Add(criteria);

    public string GetKeyCache()
        => $"JOB-#{Name.ToUpper()}#{Description.ToUpper()}#{Company.Name.ToUpper()}#{DeadLine:dd/MM/yyyy}";

    public static void JobDuplicated(INotificationHandler notification, string reference)
        => notification.RaiseError(EntityError.JobDuplicated, reference);
}
