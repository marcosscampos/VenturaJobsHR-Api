using VenturaJobsHR.Bff.CrossCutting.Enums;

namespace VenturaJobsHR.Bff.Application.Records.Job.Miscellaneous;

public class JobRequest
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SalaryRequest Salary { get; set; }
    public LocationRequest Location { get; set; }
    public CompanyRequest Company { get; set; }
    public FormOfHiringEnum FormOfHiring { get; set; }
    public OccupationAreaEnum OccupationArea { get; set; }
    public IList<CriteriaRequest> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }
}
