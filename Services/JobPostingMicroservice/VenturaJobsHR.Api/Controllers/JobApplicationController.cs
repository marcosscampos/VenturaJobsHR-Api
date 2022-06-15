using Microsoft.AspNetCore.Mvc;
using System.Net;
using VenturaJobsHR.Api.Common.Responses;
using VenturaJobsHR.Api.Common.Security;
using VenturaJobsHR.Application.Records.Jobs;
using VenturaJobsHR.Application.Services.Interfaces;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Responses;
using VenturaJobsHR.Domain.Aggregates.JobApplicationAgg.Commands;

namespace VenturaJobsHR.Api.Controllers;

[ApiVersion("1.0")]
[Route("/v{version:apiVersion}/jobApplications")]
[VenturaAuthorize(role: "applicant")]
[ApiController]
public class JobApplicationController : BaseController
{
    private readonly IJobApplicationService _service;
    public JobApplicationController(INotificationHandler notificationHandler, IJobApplicationService service) : base(notificationHandler)
    {
        _service = service;
    }

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
    public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationCommand application)
    {
        await _service.ApplyToJob(application);
        return HandleResponse(true);
    }

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
    {
        var job = await _service.GetApplicationsFromApplicant();

        return HandleResponse(false, job);
    }
}
