using VenturaJobsHR.CrossCutting.Enums;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;

public class CreateOrUpdateJobRequest
{
    public CreateOrUpdateJobRequest()
    {
        CriteriaList = new List<CriteriaRequest>();
    }

    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SalaryRequest Salary { get; set; }
    public LocationRequest Location { get; set; }
    public CompanyRequest Company { get; set; }
    public IList<CriteriaRequest> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }

    public string GetReference()
        => $"JOB{Name.ToUpper()}#{Description.ToUpper()}#{Company.Name.ToUpper()}#{FinalDate:dd/MM/yyyy}";
}
