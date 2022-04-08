using Microsoft.AspNetCore.Mvc;
using VenturaJobsHR.Api.Common;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

namespace VenturaJobsHR.Api.Controllers;

[Route("jobs")]
[ApiController]
public class JobController : BaseController
{
    private readonly IJobService _jobService;
    public JobController(IJobService jobService, INotificationHandler notification) : base(notification) => _jobService = jobService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var job = await _jobService.GetAll();
            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }

    [HttpGet("criteria")]
    public async Task<IActionResult> GetByCriteria([FromQuery] SeachJobsQuery query)
    {
        try
        {
            var job = await _jobService.GetAllJobsByCriteria(query);
            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobCommand command)
    {
        try
        {
            await _jobService.CreateJob(command);
            return HandleResponse();
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var job = await _jobService.GetById(id);

            if (job is null)
                return NotFound($"Job not found with id #{id}");

            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateJob(UpdateJobCommand command)
    {
        try
        {
            await _jobService.UpdateJob(command);

            return HandleResponse();
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob([FromRoute] string id)
    {
        try
        {
            await _jobService.DeleteJob(id);

            return HandleResponse();
        }
        catch (Exception ex)
        {
            var error = ErrorHandlerFactory.Create(ex);
            return BadRequest(error);
        }
    }
}
