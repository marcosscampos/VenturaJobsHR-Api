using AutoMapper;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Events;
using VenturaJobsHR.Domain.SeedWork.Events;
using VenturaJobsHR.Message.Dto.Common;
using VenturaJobsHR.Message.Dto.Job;

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

        CreateMap<Job, JobDto>().ReverseMap();
        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<Criteria, CriteriaDto>().ReverseMap();
        CreateMap<Salary, SalaryDto>().ReverseMap();
        CreateMap<Location, LocationDto>().ReverseMap();

        CreateMap<MessageQueueDto<Job>, JobsCreatedEvent>();
        CreateMap<MessageQueueDto<Job>, JobsUpdatedEvent>();
        CreateMap<MessageQueueDto<Job>, JobsConsumedEvent>();

        CreateMap<MessageQueueDto<JobDto>, JobsCreatedEvent>();
        CreateMap<MessageQueueDto<JobDto>, JobsUpdatedEvent>();
        CreateMap<MessageQueueDto<JobDto>, JobsConsumedEvent>();
    }
}
