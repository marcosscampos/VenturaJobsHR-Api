using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Api.Common.Responses;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Pagination;
using VenturaJobsHR.CrossCutting.Responses;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Commands;
using VenturaJobsHR.Domain.Aggregates.JobsAgg.Queries;

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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByCriteria([FromQuery] SearchJobsQuery query)
    {
        var job = await _jobService.GetAllJobsByCriteriaAndPaged(query);
        return HandleResponse(job);
    }

    /// <summary>
    /// Retorna as vagas que a empresa cadastrou
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("company")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetJobsByCompany()
    {
        var jobs = await _jobService.GetJobsByToken();
        return HandleResponse(jobs);
    }

    /// <summary>
    /// Retorna os candidatos que aplicaram a vaga e a média
    /// </summary>
    /// <param name="id">id da vaga</param>
    /// <response code="200">Retorna os candidatos</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("{id}/applications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetJobApplications([FromRoute] string id)
    {
        var jobs = await _jobService.GetApplicationsByToken(id);
        return HandleResponse(jobs);
    }

    /// <summary>
    /// Cria uma vaga
    /// </summary>
    /// <response code="201">Vaga criada com sucesso</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateJob(CreateJobCommand command)
    {
        await _jobService.CreateJob(command);
        return HandleResponse();
    }

    /// <summary>
    /// Busca uma vaga pelo id
    /// </summary>
    /// <response code="200">Retorna a vaga</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company, applicant")]
    [ProducesResponseType(typeof(GetJobsRecord), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var job = await _jobService.GetById(id);

        return HandleResponse(job);
    }

    /// <summary>
    /// Atualiza uma ou mais vagas
    /// </summary>
    /// <response code="200">Vaga atualizada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateJob(UpdateJobCommand command)
    {
        await _jobService.UpdateJob(command);

        return HandleResponse();
    }

    /// <summary>
    /// Desativa/arquiva uma vaga (deleção lógica)
    /// </summary>
    /// <response code="200">Vaga arquivada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut("active")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LogicalJobRemove(ActiveJobRecord job)
    {
        await _jobService.LogicalDeleteJob(job);

        return HandleResponse();
    }

    /// <summary>
    /// Remove uma vaga (Deleção física)
    /// </summary>
    /// <response code="200">Vaga deletada com sucesso</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteJob([FromRoute] string id)
    {
        await _jobService.DeleteJob(id);

        return HandleResponse();
    }
}
