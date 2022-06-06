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

    protected List<ApplicationResponse> CreateJobApplicationList(List<JobApplication> jobApplications, Job job, User user)
    {
        var app = new List<ApplicationResponse>();
        var userRecord = new UserRecord(user.Name, user.Phone, user.Email, user.UserType);
        foreach (var item in jobApplications)
        {
            var criteriaList = new List<JobApplicationCriteriaRecord>();
            var averageSum = new List<double>();

            foreach (var crit in item.CriteriaList)
            {
                var criteria = new JobApplicationCriteriaRecord(crit.CriteriaId, crit.ProfileType);
                var jobCriteria = job.CriteriaList.FirstOrDefault(x => x.Id == crit.CriteriaId);

                double averageMultiply = Job.GetProfileTypeBy(crit.ProfileType) * jobCriteria.Weight;

                averageSum.Add(averageMultiply);
                criteriaList.Add(criteria);
            }

            double average = averageSum.Sum();
            double profileAverage = Math.Round(average / 11, 2);

            var applicationResponse = new ApplicationResponse(userRecord, item.JobId, criteriaList, profileAverage);

            app.Add(applicationResponse);
        }

        return app;
    }

    protected List<GetJobsRecord> CreateList(List<Job> jobs)
    {
        var jobsRecord = new List<GetJobsRecord>();
        foreach (var job in jobs)
        {
            var salary = new SalaryRecord(job.Salary.Value);
            var location = new LocationRecord(job.Location.City, job.Location.State, job.Location.Country);
            var company = new CompanyRecord(job.Company.Id, job.Company.Uid, job.Company.Name);
            var criteriaList = new List<CriteriaRecord>();
            var averageSum = new List<double>();

            foreach (var criteria in job.CriteriaList)
            {
                var criteriaRecord = new CriteriaRecord(criteria.Id, criteria.Name, criteria.Description, criteria.Profiletype, criteria.Weight);

                double averageMultiply = Job.GetProfileTypeBy(criteria.Profiletype) * criteria.Weight;
                averageSum.Add(averageMultiply);

                criteriaList.Add(criteriaRecord);
            }

            double average = averageSum.Sum();
            double profileAverage = Math.Round(average / 11, 2);

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

    protected GetJobsRecord CreateObject(Job job)
    {
        var salary = new SalaryRecord(job.Salary.Value);
        var location = new LocationRecord(job.Location.City, job.Location.State, job.Location.Country);
        var company = new CompanyRecord(job.Company.Id, job.Company.Uid, job.Company.Name);
        var criteriaList = new List<CriteriaRecord>();
        var averageSum = new List<double>();

        foreach (var criteria in job.CriteriaList)
        {
            var criteriaRecord = new CriteriaRecord(criteria.Id, criteria.Name, criteria.Description, criteria.Profiletype, criteria.Weight);

            double averageMultiply = Job.GetProfileTypeBy(criteria.Profiletype) * criteria.Weight;
            averageSum.Add(averageMultiply);

            criteriaList.Add(criteriaRecord);
        }

        double average = averageSum.Sum();
        double profileAverage = Math.Round(average / 11, 2);

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
