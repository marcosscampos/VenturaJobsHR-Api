


using Newtonsoft.Json;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class CreateOrUpdateJobRequest
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SalaryRequest Salary { get; set; }
    public DateTime FinalDate { get; set; }

    [JsonIgnore]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
