using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Bff.Application.Records.User;
using VenturaJobsHR.Bff.CrossCutting.Enums;
using VenturaJobsHR.Bff.CrossCutting.Http.Extensions;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;
using VenturaJobsHR.Bff.Domain.Factory;
using VenturaJobsHR.Bff.Domain.Models.Users;

namespace VenturaJobsHR.Bff.v1.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly HttpClient _httpClient;
    public UsersController(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.GetClient(HttpClientKeysEnum.Users);

    /// <summary>
    /// Retorna todos os usuários da base
    /// </summary>
    /// <response code="200">Retorna todos os usuários da base de dados</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAllAsync() 
        => Ok(await _httpClient.GetAsync<List<User>>("v1/users"));

    /// <summary>
    /// Retorna um usuário pelo id do mesmo
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <response code="200">Retorna o usuário</response>
    /// <response code="404">Não foi encontrado nenhum registro com esse ID</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id) 
        => Ok(await _httpClient.GetSingleAsync<User>($"v1/users/{id}"));

    /// <summary>
    /// Cria um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="201">Registro adicionado com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRecord user)
        => Ok(await _httpClient.PostAsync("v1/users", user));

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRecord user)
        => Ok(await _httpClient.PutAsync("v1/users", user));

    /// <summary>
    /// Ativa ou desativa o usuário da base (Soft delete)
    /// </summary>
    /// <param name="user">ID do usuário e um booleano indicando se o mesmo vai ser ativo ou não</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ActiveUserAsync([FromBody] ActiveUserRecord user)
        => Ok(await _httpClient.PutAsync("v1/users/active", user));
}
