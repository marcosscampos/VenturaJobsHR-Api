using System.Net;
using Microsoft.AspNetCore.Mvc;
using VenturaJobsHR.Bff.Application.Records.Applications;
using VenturaJobsHR.Bff.Application.Records.Job;
using VenturaJobsHR.Bff.Common;
using VenturaJobsHR.Bff.CrossCutting.Enums;
using VenturaJobsHR.Bff.CrossCutting.Http.Extensions;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses.Handler;
using VenturaJobsHR.Bff.Domain.Factory;

namespace VenturaJobsHR.Bff.v1.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobApplications")]
[ApiController]
public class JobApplicationController : BaseController
{
    private readonly HttpClient _httpClient;

    public JobApplicationController(IHttpClientFactory httpClientFactory) =>
        _httpClient = httpClientFactory.GetClient(HttpClientKeysEnum.Jobs);

    /// <summary>
    /// O candidato se aplica a vaga
    /// </summary>
    /// <response code="201">Aplicação a vaga criada com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationRecord application)
        => ReturnObjectResult(await _httpClient.PostAsync(Endpoints.ApplicationEndpoint, application), true);

    /// <summary>
    /// Retorna as vagas que o candidato se candidatou
    /// </summary>
    /// <response code="200">Retorna as vagas</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetApplicationJobsRecord>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetApplications()
        => ReturnObjectResult(await _httpClient.GetAsync<List<GetApplicationJobsRecord>>(Endpoints.ApplicationEndpoint), false);
}