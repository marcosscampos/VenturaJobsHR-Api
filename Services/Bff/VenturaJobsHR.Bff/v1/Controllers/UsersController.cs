using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Bff.Application.Records.User;
using VenturaJobsHR.Bff.Common;
using VenturaJobsHR.Bff.CrossCutting.Enums;
using VenturaJobsHR.Bff.CrossCutting.Http.Extensions;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;
using VenturaJobsHR.Bff.Domain.Factory;
using VenturaJobsHR.Bff.Domain.Models.Users;

namespace VenturaJobsHR.Bff.v1.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/users")]
[ApiController]
public class UsersController : BaseController
{
    private readonly HttpClient _httpClient;
    public UsersController(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.GetClient(HttpClientKeysEnum.Users);

    /// <summary>
    /// Retorna os usuários do tipo empresa
    /// </summary>
    /// <response code="200">Retorna todos os usuários do tipo empresa que consta na base de dados</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAllAsync() 
        => ReturnObjectResult(await _httpClient.GetAsync<List<User>>(Endpoints.UserEndpoint));

    /// <summary>
    /// Retorna um usuário pelo id do mesmo
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <response code="200">Retorna o usuário</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id) 
        => ReturnObjectResult(await _httpClient.GetAsync<User>(string.Concat(Endpoints.UserEndpoint, $"/{id}")));

    /// <summary>
    /// Retorna o usuário baseado no user_id que contém no token retornado no frontend
    /// </summary>
    /// <response code="200">Retorna o usuário</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpGet("user-token")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUserByToken()
        => ReturnObjectResult(await _httpClient.GetAsync<User>(string.Concat(Endpoints.UserEndpoint, "/user-token")));

    /// <summary>
    /// Cria um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="201">Registro adicionado com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRecord user)
        => ReturnObjectResult(await _httpClient.PostAsync(Endpoints.UserEndpoint, user));

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRecord user)
        => ReturnObjectResult(await _httpClient.PutAsync(Endpoints.UserEndpoint, user));

    /// <summary>
    /// Ativa ou desativa o usuário da base (Soft delete)
    /// </summary>
    /// <param name="user">ID do usuário e um booleano indicando se o mesmo vai ser ativo ou não</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <response code="401">Caso o token esteja incorreto ou faltando alguma informação importante</response>
    /// <response code="403">Caso seu acesso não seja permitido nesse endpoint</response>
    /// <response code="404">Caso não tenha encontrado o usuário na base de dados</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ForbiddenResponse), (int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(NotFoundResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ActiveUserAsync([FromBody] ActiveUserRecord user)
        => ReturnObjectResult(await _httpClient.PutAsync(string.Concat(Endpoints.UserEndpoint, "/active"), user));
}
