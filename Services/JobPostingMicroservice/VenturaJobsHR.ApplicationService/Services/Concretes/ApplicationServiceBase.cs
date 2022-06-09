using System;
using System.Collections.Generic;
using System.Linq;
using VenturaJobsHR.Application.Records.Applications;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Records.Jobs.Miscellaneous;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Entities;

namespace VenturaJobsHR.Application.Services.Concretes;

public class ApplicationServiceBase
{
    protected INotificationHandler Notification;

    protected ApplicationServiceBase(INotificationHandler notification)
    {
        Notification = notification;
    }

    protected static List<ApplicationResponse> CreateJobApplicationList(IEnumerable<JobApplication> jobApplications,
        Job job, User user)
    {
        var userRecord = new UserRecord(user.Name, user.Phone, user.Email, user.UserType, user.Active);

        return (from item in jobApplications
            let criteriaList = item.CriteriaList.Select(crit => 
                    new JobApplicationCriteriaRecord(crit.CriteriaId, crit.ProfileType)).ToList()
            let average = item.CriteriaList.Sum(x =>
                              Job.GetProfileTypeBy(x.ProfileType) *
                              job.CriteriaList.FirstOrDefault(p => p.Id == x.CriteriaId)!.Weight) /
                          (double)job.CriteriaList.Sum(x => x.Weight)
            let profileAverage = Math.Round(average, 2)
            select new ApplicationResponse(userRecord, item.JobId, criteriaList, profileAverage)).ToList();
    }

    protected static List<GetApplicationJobsRecord> CreateApplicationJobsList(IEnumerable<Job> jobs) =>
        (from job in jobs
            let salary = new SalaryRecord(job.Salary.Value)
            let location = new LocationRecord(job.Location.City, job.Location.State, job.Location.Country)
            select new GetApplicationJobsRecord(
                job.Id, 
                job.Name, 
                job.Description, 
                salary, 
                location, 
                job.Status,
                job.DeadLine)).ToList();

    protected static List<GetJobsRecord> CreateList(List<Job> jobs)
    {
        var jobsRecord = new List<GetJobsRecord>();
        foreach (var job in jobs)
        {
            var salary = new SalaryRecord(job.Salary.Value);
            var location = new LocationRecord(job.Location.City, job.Location.State, job.Location.Country);
            var company = new CompanyRecord(job.Company.Id, job.Company.Uid, job.Company.Name);
            var criteriaList = job.CriteriaList.Select(criteria =>
                new CriteriaRecord(
                    criteria.Id,
                    criteria.Name,
                    criteria.Description,
                    criteria.Profiletype,
                    criteria.Weight)).ToList();

            var average = job.CriteriaList.Sum(x =>
                Job.GetProfileTypeBy(x.Profiletype) * x.Weight) / (double)job.CriteriaList.Sum(x => x.Weight);

            var profileAverage = Math.Round(average, 2);

            var jobRecord = new GetJobsRecord(
                job.Id,
                job.Name,
                job.Description,
                salary,
                location,
                company,
                job.FormOfHiring,
                criteriaList,
                job.Status,
                job.OccupationArea,
                job.DeadLine,
                profileAverage,
                job.Active);

            jobsRecord.Add(jobRecord);
        }

        return jobsRecord;
    }

    protected static GetJobsRecord CreateObject(Job job)
    {
        var salary = new SalaryRecord(job.Salary.Value);
        var location = new LocationRecord(job.Location.City, job.Location.State, job.Location.Country);
        var company = new CompanyRecord(job.Company.Id, job.Company.Uid, job.Company.Name);
        var criteriaList = job.CriteriaList.Select(criteria =>
            new CriteriaRecord(
                criteria.Id,
                criteria.Name,
                criteria.Description,
                criteria.Profiletype,
                criteria.Weight)).ToList();

        var averageWeight = job.CriteriaList.Sum(x => Job.GetProfileTypeBy(x.Profiletype) * x.Weight) /
                               (double)job.CriteriaList.Sum(x => x.Weight);
        var profileAverage = Math.Round(averageWeight, 2);

        var jobRecord = new GetJobsRecord(
            job.Id,
            job.Name,
            job.Description,
            salary,
            location,
            company,
            job.FormOfHiring,
            criteriaList,
            job.Status,
            job.OccupationArea,
            job.DeadLine,
            profileAverage,
            job.Active);

        return jobRecord;
    }
}