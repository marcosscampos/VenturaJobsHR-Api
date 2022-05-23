using VenturaJobsHR.Bff.CrossCutting.Enums;
using VenturaJobsHR.Bff.Domain.Seedwork.Entity;

namespace VenturaJobsHR.Bff.Domain.Models.Jobs;

public class Job : Entity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Salary Salary { get; set; }
    public Location Location { get; set; }
    public Company Company { get; set; }
    public FormOfHiringEnum FormOfHiring { get; set; }
    public IList<Criteria> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }
    public bool Active { get; set; }
}
