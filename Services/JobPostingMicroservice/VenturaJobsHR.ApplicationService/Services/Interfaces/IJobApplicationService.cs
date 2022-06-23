using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;

namespace VenturaJobsHR.Application.Services.Interfaces;

public interface IJobApplicationService
{
    Task<Pagination<GetApplicationJobsRecord>> GetApplicationsFromApplicant(SearchJobsQuery query);

    Task ApplyToJob(CreateJobApplicationCommand job);
}
