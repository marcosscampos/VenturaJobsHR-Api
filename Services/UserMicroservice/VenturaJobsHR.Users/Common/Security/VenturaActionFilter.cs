namespace VenturaJobsHR.Users.Common.Security;

public class VenturaActionFilter : IAsyncAuthorizationFilter
{
    private readonly string _role;
    private readonly IUserRepository _userRepository;
    public VenturaActionFilter(string role, IUserRepository userRepository)
    {
        _role = role;
        _userRepository = userRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authHeader = context.HttpContext.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(authHeader))
            throw new ForbiddenException("Missing token");

        var roles = _role.Split(",", StringSplitOptions.TrimEntries);
        var uid = context.HttpContext.User.FindFirst("user_id");

        if (uid is null)
            throw new NotFoundException("UID not found.");

        var user = await _userRepository.GetUserByFireBaseToken(uid.Value);
        if (user is null)
            return;

        var userType = User.GetUserTypeBy(user.UserType);
        if (roles.All(role => !userType.Equals(role)))
        {
            throw new ForbiddenException("Role is not match with this endpoint or is invalid.");
        }
    }
}
