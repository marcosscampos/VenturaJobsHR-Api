using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Records.Jobs.Miscellaneous;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands.Requests;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

namespace VenturaJobsHR.Tests.Resources.Data;

public static class DataBuilder
{
    public async static Task<Pagination<GetJobsRecord>> GetAllJobsPaged()
    {
        var jobRecord1 = new GetJobsRecord(
            "1",
            "Desenvolvedor .NET",
            "Lorem Ipsum",
            new SalaryRecord(3500),
            new LocationRecord("Rio de Janeiro", "RJ", "Brasil"),
            new CompanyRecord("1", "KADSOKPADSPKOADSKOP", "Campos LTDA"),
            FormOfHiringEnum.Autonomous,
            new List<CriteriaRecord>(),
            JobStatusEnum.Published,
            OccupationAreaEnum.Development,
            DateTime.Now.AddDays(10),
            3.25,
            true);
        var jobRecordList = new List<GetJobsRecord> { jobRecord1 };
        return new Pagination<GetJobsRecord>
        {
            Data = jobRecordList,
            Total = jobRecordList.Count,
            PageSize = 10,
            CurrentPage = 1
        };
    }

    public async static Task<CreateJobCommand> GetJobCommand()
    {
        var salary = new SalaryRequest
        {
            Value = 3500
        };
        var location = new LocationRequest
        {
            City = "Rio de Janeiro",
            State = "Rj",
            Country = "Brasil"
        };

        var company = new CompanyRequest
        {
            Id = "1",
            Name = "Marcos LTDA",
            Uid = "KADSKÇLJASDJKLASDKLJ"
        };

        var criteriaList = new List<CriteriaRequest>
        {
            new CriteriaRequest
            {
                Id = "1",
                Name = "Desenvolver de olhos vendados",
                Description = "Ou ser cego",
                Profiletype = ProfileTypeEnum.Relevant,
                Weight = 4
            }
        };

        return new CreateJobCommand()
        {
            JobList = new List<CreateOrUpdateJobRequest>
            {
                new CreateOrUpdateJobRequest
                {
                    Id = "1",
                    Name = "Desenvolvedor .NET",
                    Description = "Lorem Ipsum",
                    Salary = salary,
                    Location = location,
                    Company = company,
                    FormOfHiring = FormOfHiringEnum.Autonomous,
                    OccupationArea = OccupationAreaEnum.Development,
                    CriteriaList = criteriaList,
                    DeadLine = DateTime.Now.AddDays(10),
                    Status = JobStatusEnum.Published
                }
            }
        };
    }
    
    public async static Task<Pagination<Job>> GetAllJobs()
    {
        var jobRecord1 = new Job(
            "1",
            "Desenvolvedor .NET",
            "Lorem Ipsum",
            new Salary(3500),
            new Location("Rio de Janeiro", "RJ", "Brasil"),
            new Company("1", "KADSOKPADSPKOADSKOP", "Campos LTDA"),
            FormOfHiringEnum.Autonomous,
            OccupationAreaEnum.Development,
            JobStatusEnum.Published,
            DateTime.Now.AddDays(10)
        );
        var jobRecordList = new List<Job> { jobRecord1 };
        return new Pagination<Job>
        {
            Data = jobRecordList,
            Total = jobRecordList.Count,
            PageSize = 10,
            CurrentPage = 1
        };
    }

    public async static Task<GetJobsRecord> GetSingleJob()
    {
        return new GetJobsRecord(
            "1",
            "Desenvolvedor .NET",
            "Lorem Ipsum",
            new SalaryRecord(3500),
            new LocationRecord("Rio de Janeiro", "RJ", "Brasil"),
            new CompanyRecord("1", "KADSOKPADSPKOADSKOP", "Campos LTDA"),
            FormOfHiringEnum.Autonomous,
            new List<CriteriaRecord>(),
            JobStatusEnum.Published,
            OccupationAreaEnum.Development,
            DateTime.Now,
            3.25,
            true);
    }
}