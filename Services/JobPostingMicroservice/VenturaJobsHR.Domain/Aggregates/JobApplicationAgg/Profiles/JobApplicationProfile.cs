using AutoMapper;
using VenturaJobsHR.Domain.Aggregates.Common.Events;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Events;

namespace VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Profiles;

public class JobApplicationProfile : Profile
{
    public JobApplicationProfile()
    {
        CreateMap<JobApplication, BaseJobApplicationEventObject>().ReverseMap();
        CreateMap<JobApplication, BaseNotification<BaseJobApplicationEventObject>>();

        CreateMap<JobApplication, JobApplicationCreatedEvent>();
    }
}
