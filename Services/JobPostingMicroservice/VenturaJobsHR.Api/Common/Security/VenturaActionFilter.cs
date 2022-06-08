﻿using Microsoft.AspNetCore.Mvc.Filters;
using VenturaJobsHR.Common.Exceptions;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

namespace VenturaJobsHR.Api.Common.Security;

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

        var user = await _userRepository.GetUserByFirebaseId(uid.Value);

        if (roles.Any(role => !User.GetUserTypeBy(user.UserType).Equals(role) || !role.Equals("allowAnonymous")))
        {
            throw new ForbiddenException("Role is not match with this endpoint or is invalid.");
        }
    }
}