using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Bff.Application.Records.Applications;
using VenturaJobsHR.Bff.Application.Records.Job;
using VenturaJobsHR.Bff.Application.Records.User;
using VenturaJobsHR.Bff.Common;
using VenturaJobsHR.Bff.CrossCutting.Enums;
using VenturaJobsHR.Bff.CrossCutting.Http.Extensions;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses.Handler;
using VenturaJobsHR.Bff.CrossCutting.Pagination;
using VenturaJobsHR.Bff.Domain.Factory;
using VenturaJobsHR.Bff.Domain.Models.Jobs;
using VenturaJobsHR.Bff.Domain.Seedwork.Query;

namespace VenturaJobsHR.Bff.v1.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobs")]
[ApiController]
public class JobsController : BaseController
{
    private readonly HttpClient _httpClient;
    public JobsController(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.GetClient(HttpClientKeysEnum.Jobs);

    /// <summary>
    /// Busca vagas baseadas no filtro utilizado
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByCriteria([FromQuery] SearchJobsQuery query)
        => ReturnObjectResult(await _httpClient.GetAsync<object>(Endpoints.JobEndpointWithQuery(query)), false);

    /// <summary>
    /// Retorna as vagas que a empresa cadastrou
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpGet("company")]
    [ProducesResponseType(typeof(Pagination<GetJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetJobsByCompany()
        => ReturnObjectResult(await _httpClient.GetAsync<object>(string.Concat(Endpoints.JobEndpoint, "/company")), false);

    /// <summary>
    /// Cria uma vaga
    /// </summary>
    /// <response code="201">Vaga criada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateJob(CreateJobRecord command)
        => ReturnObjectResult(await _httpClient.PostAsync(Endpoints.JobEndpoint, command), true);

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
    [ProducesResponseType(typeof(List<ApplicationResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetJobApplications([FromRoute] string id)
        => ReturnObjectResult(await _httpClient.GetAsync<object>(string.Concat(Endpoints.JobEndpoint, $"/{id}/job-applications")), false);

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
    [ProducesResponseType(typeof(JobReportRecord), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetJobApplicationReports([FromRoute] string id)
        => ReturnObjectResult(await _httpClient.GetAsync<object>(string.Concat(Endpoints.JobEndpoint, $"/{id}/job-report")), false);


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
    [ProducesResponseType(typeof(GetJobsRecord), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById([FromRoute] string id)
        => ReturnObjectResult(await _httpClient.GetAsync<GetJobsRecord>(string.Concat(Endpoints.JobEndpoint, $"/{id}")), false);

    /// <summary>
    /// Atuliza uma ou mais vagas
    /// </summary>
    /// <response code="200">Vaga atualizada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateJob(UpdateJobRecord command)
        => ReturnObjectResult(await _httpClient.PutAsync(Endpoints.JobEndpoint, command), false);

    /// <summary>
    /// Desativa/arquiva uma vaga (deleção lógica)
    /// </summary>
    /// <response code="200">Vaga arquivada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> LogicalJobRemove(ActiveJobRecord job)
        => ReturnObjectResult(await _httpClient.PutAsync(string.Concat(Endpoints.JobEndpoint, "/active"), job), false);

    /// <summary>
    /// Cancelar a publicação de uma vaga
    /// </summary>
    /// <param name="id">id da vaga</param>
    /// <response code="200">Vaga cancelada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut("close")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> CloseJobPosting([FromBody] CloseJobRecord id)
        => ReturnObjectResult(await _httpClient.PutAsync(string.Concat(Endpoints.JobEndpoint, "/close"), id), false);
    
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
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateDeadLine([FromBody] UpdateDeadLineRecord command)
        => ReturnObjectResult(await _httpClient.PatchAsync(string.Concat(Endpoints.JobEndpoint, "/renew"), command), false);
}
