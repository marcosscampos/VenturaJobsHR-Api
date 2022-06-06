using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Api.Common.Responses;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Responses;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Entities;

namespace VenturaJobsHR.Api.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobApplications")]
[ApiController]
public class JobApplicationController : BaseController
{
    private readonly IJobApplicationService _service;
    public JobApplicationController(INotificationHandler notificationHandler, IJobApplicationService service) : base(notificationHandler)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "applicant")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationCommand application)
    {
        await _service.ApplyToJob(application);
        return HandleResponse();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "applicant")]
    [ProducesResponseType(typeof(JobApplication), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetApplications()
    {
        var job = await _service.GetApplicationsFromApplicant();

        return HandleResponse(job);
    }
}
