using AutoMapper;
using VenturaJobsHR.Domain.Aggregates.Common.Events;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Profiles;

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
