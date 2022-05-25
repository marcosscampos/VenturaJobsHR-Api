namespace VenturaJobsHR.Users.Common.Security;

public class VenturaActionFilter : IAsyncAuthorizationFilter
{
    private readonly string _role;
    public VenturaActionFilter(string role)
    {
        _role = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string authHeader = context.HttpContext.Request.Headers["Authorization"];
        var roles = _role.Split(",", StringSplitOptions.TrimEntries);

        if (authHeader is null)
        {
            throw new ForbiddenException("Authorization Header is missing");
        }

        if (!Contains(roles, "company") || !Contains(roles, "applicant"))
        {
            throw new UnauthorizedException("Invalid or missing role");
        }
    }


    private bool Contains(string[] values, string value) => values.Contains(value);
}
