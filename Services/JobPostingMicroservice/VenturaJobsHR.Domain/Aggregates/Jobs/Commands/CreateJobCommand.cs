﻿using FluentValidation;
using MediatR;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.Domain.Aggregates.Common.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands.Requests;
using VenturaJobsHR.Domain.SeedWork.Validators;

namespace VenturaJobsHR.Domain.Aggregates.Jobs.Commands;

public class CreateJobCommand : BaseJobCommand, IRequest<Unit>
{
    public override bool IsValid()
    {
        ValidationResult = new CreateJobValidator().Validate(this);

        return ValidationResult.IsValid;
    }
}

public class CreateJobValidator : BaseValidator<CreateJobCommand>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.JobList.Select(p => new { Name = p.Name, Description = p.Description, FinalDate = p.FinalDate }).ToList())
            .Cascade(CascadeMode.Stop)
            .Must(list => !list.GroupBy(x => x).Any(y => y.Count() > 1)).WithState(x => AddCommandErrorObject(EntityError.DuplicatedItems));

        RuleForEach(x => x.JobList).ChildRules(job =>
        {
            job.RuleFor(x => x.Salary).SetValidator(x => new SalaryValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Salary).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidSalary, x.GetReference()));
            });

            job.RuleFor(x => x.Location).SetValidator(x => new LocationValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Location).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidLocation, x.GetReference()));
            });

            job.RuleFor(x => x.Company).SetValidator(x => new CompanyValidator(x.GetReference())).DependentRules(() =>
            {
                job.RuleFor(x => x.Company).NotNull().WithState(x => AddCommandErrorObject(EntityError.JobInvalidCompany, x.GetReference()));
            });

            job.RuleFor(x => x.Name).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobName, x.GetReference()));
            job.RuleFor(x => x.Description).NotEmpty().WithState(x => AddCommandErrorObject(EntityError.InvalidJobDescription, x.GetReference()));
            job.RuleFor(x => x.FinalDate).Must(x => x != DateTime.MinValue).WithState(x => AddCommandErrorObject(EntityError.SalaryNotZero, x.GetReference()));
        });
    }
}

