using VenturaJobsHR.Common.Extensions;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.Common.Entities;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

public class Job : Entity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Salary Salary { get; set; }
    public Location Location { get; set; }
    public Company Company { get; set; }
    public IList<Criteria> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }


    public Job(string name, string description, Salary salary, Location location, Company company, JobStatusEnum status, DateTime finalDate)
    {
        Name = name;
        Description = description;
        Salary = salary;
        Location = location;
        Company = company;
        CriteriaList = new List<Criteria>();
        Status = status;
        FinalDate = finalDate;
        CreationDate = new DateTimeWithZone(DateTime.Now).LocalTime;
    }

    public Job(string id, string name, string description, Salary salary, Location location, Company company, JobStatusEnum status, DateTime finalDate)
        : this(name, description, salary, location, company, status, finalDate)
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
            DateTime finalDate
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Salary = salary;
        Location = location;
        Company = company;
        Status = status;
        FinalDate = finalDate;
        LastUpdate = new DateTimeWithZone(DateTime.Now).LocalTime;
    }

    public Job() { }

    public void AddCriteria(Criteria criteria)
        => CriteriaList.Add(criteria);

    public string GetKeyCache()
        => $"JOB{Name.ToUpper()}#{Description.ToUpper()}#{Company.Name.ToUpper()}#{FinalDate:dd/MM/yyyy}";
}
