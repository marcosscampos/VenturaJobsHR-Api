using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Api.Common.Responses;
using VenturaJobsHR.Api.Common.Security;
using VenturaJobsHR.Application.Records.Applications;
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
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByCriteria([FromQuery] SearchJobsQuery query)
    {
        var job = await _jobService.GetAllJobsByCriteriaAndPaged(query);
        return HandleResponse(false, job);
    }
    
    /// <summary>
    /// Número de vagas criadas hoje
    /// </summary>
    /// <response code="200">Retorna as quantidade</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet("created-today")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetJobsCreatedToday()
    {
        var job = await _jobService.NumberOfJobsCreatedToday();
        return HandleResponse(false, job);
    }
    
    /// <summary>
    /// Número de vagas criadas até hoje
    /// </summary>
    /// <response code="200">Retorna a quantidade</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet("jobs-created")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetJobsCreated()
    {
        var job = await _jobService.NumberOfJobsCreated();
        return HandleResponse(false, job);
    }

    /// <summary>
    /// Retorna as vagas que a empresa cadastrou
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpGet("company")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetJobsByCompany([FromQuery] SearchJobsQuery query)
    {
        var jobs = await _jobService.GetJobsByToken(query);
        return HandleResponse(false, jobs);
    }
    
    /// <summary>
    /// Verifica se o usuário já se candidatou a vaga
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("{jobId}/can-apply")]
    [VenturaAuthorize(role: "applicant, company")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> AlreadyApplied([FromRoute] string jobId)
    {
        var alreadyApplied = await _jobService.CanApplyToJob(jobId);
        return HandleResponse(false, alreadyApplied);
    }

    /// <summary>
    /// Retorna os candidatos que aplicaram a vaga e a média
    /// </summary>
    /// <param name="id">id da vaga</param>
    /// <response code="200">Retorna os candidatos</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("{id}/job-applications")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(List<ApplicationResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetJobApplications([FromRoute] string id)
    {
        var jobs = await _jobService.GetApplicationsByToken(id);
        return HandleResponse(false, jobs);
    }
    
    /// <summary>
    /// Retorna o relatório da vaga 
    /// </summary>
    /// <param name="id">id da vaga</param>
    /// <response code="200">Retorna o relatório em relação a média da vaga</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("{id}/job-report")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(JobReportRecord), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetJobApplicationReports([FromRoute] string id)
    {
        var jobs = await _jobService.GetJobReport(id);
        return HandleResponse(false, jobs);
    }

    /// <summary>
    /// Cria uma vaga
    /// </summary>
    /// <response code="201">Vaga criada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPost]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateJob(CreateJobCommand command)
    {
        await _jobService.CreateJob(command);
        return HandleResponse(true);
    }

    /// <summary>
    /// Busca uma vaga pelo id
    /// </summary>
    /// <response code="200">Retorna a vaga</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [VenturaAuthorize(role: "company, applicant, admin")]
    [ProducesResponseType(typeof(GetJobsRecord), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var job = await _jobService.GetById(id);

        return HandleResponse(false, job);
    }

    /// <summary>
    /// Atualiza uma ou mais vagas
    /// </summary>
    /// <response code="204">Vaga atualizada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateJob(UpdateJobCommand command)
    {
        await _jobService.UpdateJob(command);

        return HandleResponse(false);
    }

    /// <summary>
    /// Desativa/arquiva uma vaga (deleção lógica)
    /// </summary>
    /// <response code="204">Vaga arquivada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPut("active")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> LogicalJobRemove([FromBody] ActiveJobRecord job)
    {
        await _jobService.LogicalDeleteJob(job);

        return HandleResponse(false);
    }

    /// <summary>
    /// Fechar uma vaga
    /// </summary>
    /// <param name="id">id da vaga</param>
    /// <response code="204">Vaga fechada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut("close")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> CloseJobPosting([FromBody] CloseJobRecord id)
    {
        await _jobService.CloseJobPosting(id);

        return HandleResponse(false);
    }
    
    /// <summary>
    /// Atualizar a data limite de uma vaga
    /// </summary>
    /// <response code="204">Data limite atualizada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPatch("renew")]
    [VenturaAuthorize(role: "company")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateDeadLine([FromBody] UpdateDeadLineCommand command)
    {
        await _jobService.UpdateDeadLineJobPosting(command);

        return HandleResponse(false);
    }
}
