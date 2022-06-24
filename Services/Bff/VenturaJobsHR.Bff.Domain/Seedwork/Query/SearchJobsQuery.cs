using VenturaJobsHR.Bff.CrossCutting.Pagination;
using VenturaJobsHR.Bff.Domain.Models.Jobs;

namespace VenturaJobsHR.Bff.Domain.Seedwork.Query;

public class SearchJobsQuery : BaseListQuery<Pagination<Job>>
{
    public decimal Salary { get; set; }
    public DateTime DeadLine { get; set; }
    public int OccupationArea { get; set; }
}
