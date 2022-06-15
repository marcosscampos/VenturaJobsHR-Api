using System.Net;
using Microsoft.AspNetCore.Mvc;
using VenturaJobsHR.Bff.CrossCutting.Http.Responses;

namespace VenturaJobsHR.Bff.v1.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult ReturnObjectResult(object result, bool isCreated)
    {
        switch (result)
        {
            case ForbiddenResponse:
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return StatusCode(StatusCodes.Status403Forbidden, result);
            
            case BadRequestResponse:
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return StatusCode(StatusCodes.Status400BadRequest, result);
            
            case NotFoundResponse:
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return StatusCode(StatusCodes.Status404NotFound, result);

            case UnauthorizedResponse:
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return StatusCode(StatusCodes.Status401Unauthorized, result);

            default:
                if (isCreated)
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                
                Response.StatusCode = (int)HttpStatusCode.OK;
                return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}