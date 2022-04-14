using AutoMapper;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.SeedWork.Events;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Profiles;

public class JobProfile : Profile
{
    public JobProfile()
    {
        CreateMap<Job, BaseJobEventObject>();
        CreateMap<Job, BaseNotification<BaseJobEventObject>>()
            .ForMember(x => x.Event, x => x.MapFrom(x => x));

        CreateMap<JobsCreatedEvent, Salary>()
            .ForMember(x => x.Value, x => x.MapFrom(x => x.Event.Salary));
        CreateMap<JobsUpdatedEvent, Salary>();

        CreateMap<Job, JobsCreatedEvent>();
        CreateMap<Job, JobsUpdatedEvent>();
    }
}
