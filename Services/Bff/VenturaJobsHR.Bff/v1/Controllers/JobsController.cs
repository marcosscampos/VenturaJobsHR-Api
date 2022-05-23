using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Bff.Application.Records.Job;
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
public class JobsController : ControllerBase
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
    [ProducesResponseType(typeof(Pagination<Job>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByCriteria([FromQuery] SearchJobsQuery query)
        => Ok(await _httpClient.GetAsync<object>(Endpoints.JobEndpointWithQuery(query)));

    /// <summary>
    /// Cria uma vaga
    /// </summary>
    /// <response code="201">Vaga criada com sucesso</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateJob(CreateJobRecord command)
        => Ok(await _httpClient.PostAsync(Endpoints.JobEndpoint, command));

    /// <summary>
    /// Busca uma vaga pelo id
    /// </summary>
    /// <response code="200">Retorna a vaga</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Job), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] string id)
        => Ok(await _httpClient.GetAsync<Job>(string.Concat(Endpoints.JobEndpoint, $"/{id}")));

    /// <summary>
    /// Atuliza uma ou mais vagas
    /// </summary>
    /// <response code="200">Vaga atualizada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateJob(UpdateJobRecord command)
        => Ok(await _httpClient.PutAsync(Endpoints.JobEndpoint, command));

    /// <summary>
    /// Desativa/arquiva uma vaga (deleção lógica)
    /// </summary>
    /// <response code="200">Vaga arquivada com sucesso</response>
    /// <response code="404">Não foi encontrada nenhuma informação com essa identificação no banco de dados</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LogicalJobRemove(ActiveJobRecord job)
        => Ok(await _httpClient.PutAsync(string.Concat(Endpoints.JobEndpoint, "/active"), job));

    /// <summary>
    /// Remove uma vaga (Deleção física)
    /// </summary>
    /// <response code="200">Vaga deletada com sucesso</response>
    /// <response code="400">Quando alguma informação enviada para a API não satisfazer o que o mesmo está esperando</response>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(HandleResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteJob([FromRoute] string id)
        => Ok(await _httpClient.RemoveAsync<HandleResponse>(string.Concat(Endpoints.JobEndpoint, $"/{id}")));
}
