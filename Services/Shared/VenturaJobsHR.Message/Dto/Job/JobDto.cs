using VenturaJobsHR.Message.Enums;

namespace VenturaJobsHR.Message.Dto.Job;

public class JobDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SalaryDto Salary { get; set; }
    public LocationDto Location { get; set; }
    public CompanyDto Company { get; set; }
    public IList<CriteriaDto> CriteriaList { get; set; }
    public JobStatusEnum Status { get; set; }
    public DateTime FinalDate { get; set; }
}
