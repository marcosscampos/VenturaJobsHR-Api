using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using VenturaJobsHR.Api.Common.ErrorsHandler;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Responses;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

namespace VenturaJobsHR.Api.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobs")]
[ApiController]
[OpenApiTag("JobsController", Description = "Controller responsável pela gestão de jobs.")]
public class JobController : BaseController
{
    private readonly IJobService _jobService;
    public JobController(IJobService jobService, INotificationHandler notification) : base(notification) => _jobService = jobService;

    /// <summary>
    /// Lista todas as vagas
    /// </summary>
    /// <response code="200">Retorna todas as vagas</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Job>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var job = await _jobService.GetAll();
            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Busca jobs baseado nos filtros de salário e data final
    /// </summary>
    /// <response code="200">Retorna os jobs filtrados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("criteria")]
    [ProducesResponseType(typeof(List<Job>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByCriteria([FromQuery] SearchJobsQuery query)
    {
        try
        {
            var job = await _jobService.GetAllJobsByCriteriaAndPaged(query);
            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Cria um job
    /// </summary>
    /// <response code="201">Cria um ou mais jobs</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateJob(CreateJobCommand command)
    {
        try
        {
            await _jobService.CreateJob(command);
            return HandleResponse();
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Busca um job pelo id
    /// </summary>
    /// <response code="200">Busca um job pelo id</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var job = await _jobService.GetById(id);

            return HandleResponse(job);
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Atuliza um ou mais jobs
    /// </summary>
    /// <response code="200">Atualizado</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateJob(UpdateJobCommand command)
    {
        try
        {
            await _jobService.UpdateJob(command);

            return HandleResponse();
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Remove um ou mais jobs
    /// </summary>
    /// <response code="200">Deletado</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteJob([FromRoute] string id)
    {
        try
        {
            await _jobService.DeleteJob(id);

            return HandleResponse();
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }
}
