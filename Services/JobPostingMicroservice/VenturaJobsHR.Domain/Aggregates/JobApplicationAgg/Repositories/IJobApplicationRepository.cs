using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Repositories;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task<IList<JobApplication>> GetApplicationsByUserId(string userId);
}
