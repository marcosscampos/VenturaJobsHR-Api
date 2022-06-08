using System.Collections.Generic;
using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;

namespace VenturaJobsHR.Application.Services.Interfaces;

public interface IJobApplicationService
{
    Task<IList<GetApplicationJobsRecord>> GetApplicationsFromApplicant();

    Task ApplyToJob(CreateJobApplicationCommand job);
}
