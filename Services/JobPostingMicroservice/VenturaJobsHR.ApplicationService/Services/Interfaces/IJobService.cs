﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VenturaJobsHR.Application.Records.Applications;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;

namespace VenturaJobsHR.Application.Services.Interfaces;

public interface IJobService
{
    Task CreateJob(CreateJobCommand command);
    Task<GetJobsRecord> GetById(string id);
    Task<IList<GetJobsRecord>> GetJobsByToken();
    Task<IList<ApplicationResponse>> GetApplicationsByToken(string id);
    Task<JobReportRecord> GetJobReport(string id);
    Task UpdateJob(UpdateJobCommand command);
    Task LogicalDeleteJob(ActiveJobRecord job);
    Task CancelJobPosting(string id);
    Task<Pagination<GetJobsRecord>> GetAllJobsByCriteriaAndPaged(SearchJobsQuery query);
}
