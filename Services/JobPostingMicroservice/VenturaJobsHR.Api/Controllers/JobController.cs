using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Api.Common.ErrorsHandler;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.CrossCutting.Responses;
using VenturaJobsHR.Domain.Aggregates.Jobs.Commands;
using VenturaJobsHR.Domain.Aggregates.Jobs.Entities;
using VenturaJobsHR.Domain.Aggregates.Jobs.Queries;

namespace VenturaJobsHR.Api.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobs")]
[ApiController]
public class JobController : BaseController
{
    private readonly IJobService _jobService;
    public JobController(IJobService jobService, INotificationHandler notification) : base(notification) => _jobService = jobService;

    /// <summary>
    /// Busca vagas baseadas no filtro utilizado
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Pagination<Job>), (int)HttpStatusCode.OK)]
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
    /// Cria uma vaga
    /// </summary>
    /// <response code="201">Vaga criada com sucesso</response>
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
    /// Busca uma vaga pelo id
    /// </summary>
    /// <response code="200">Retorna a vaga</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Job), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.NotFound)]
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
    /// Atuliza uma ou mais vagas
    /// </summary>
    /// <response code="200">Vaga atualizada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.NotFound)]
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
    /// Desativa/arquiva uma vaga (deleção lógica)
    /// </summary>
    /// <response code="200">Vaga arquivada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LogicalJobRemove(ActiveJobRecord job)
    {
        try
        {
            await _jobService.LogicalDeleteJob(job);

            return HandleResponse();
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Remove uma vaga (Deleção física)
    /// </summary>
    /// <response code="200">Vaga deletada com sucesso</response>
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
