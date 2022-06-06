using AutoMapper;
using VenturaJobsHR.Domain.Aggregates.Common.Events;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Events;

namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Profiles;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<Job, BaseJobEventObject>().ReverseMap();
        CreateMap<Job, BaseNotification<BaseJobEventObject>>();

        CreateMap<Job, JobsCreatedEvent>();
        CreateMap<Job, JobsUpdatedEvent>();
    }
}
