using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Net;
using VenturaJobsHR.Users.Application.Records.User;
using VenturaJobsHR.Users.Application.Services.Interface;
using VenturaJobsHR.Users.Common.Handler.Errors;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.v1.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/users")]
[ApiController]
[OpenApiTag("UserController", Description = "Controller responsável pela gestão de usuários.")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retorna todos os usuários da base
    /// </summary>
    /// <response code="200">Retorna todos os usuários da base de dados</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Retorna um usuário pelo id do mesmo
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <response code="200">Retorna o usuário</response>
    /// <response code="404">Não foi encontrado nenhum registro com esse ID</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        try
        {
            var user = await _userService.GetUserBy(id);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Cria um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="201">Registro adicionado com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRecord user)
    {
        try
        {
            await _userService.CreateUserAsync(user);

            return Created("", "Processed");
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    /// <param name="user">Todos os registros do usuário</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRecord user)
    {
        try
        {
            await _userService.UpdateUserAsync(user);

            return Ok("Processed");
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }

    /// <summary>
    /// Ativa ou desativa o usuário da base (Soft delete)
    /// </summary>
    /// <param name="user">ID do usuário e um booleano indicando se o mesmo vai ser ativo ou não</param>
    /// <response code="200">Requisição concluída com sucesso</response>
    /// <response code="400">Houve uma falha na requisição. Alguma informação não está de acordo com o que devia ser enviado para a API</response>
    /// <returns></returns>
    [HttpPut("active")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorHandler), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ActiveUserAsync([FromBody] ActiveUserRecord user)
    {
        try
        {
            await _userService.ActivateUserAsync(user);

            return Ok("Processed");
        }
        catch (Exception ex)
        {
            return ErrorResult.ReturnErrorResult(ex);
        }
    }
}
