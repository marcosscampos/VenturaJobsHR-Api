using VenturaJobsHR.Users.Application.Records;
using VenturaJobsHR.Users.Application.Services.Interface;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.v1.Modules;

public class UserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/user", async (IUserService service) =>
        {
            var users = await service.GetUsersAsync();

            Results.Ok(users);
        });

        app.MapGet("/v1/user/{id}", async (IUserService service, string id) =>
        {
            var user = await service.GetUserBy(id);

            Results.Ok(user);
        });

        app.MapPost("/v1/user", async (IUserService service, User user) =>
        {
            await service.CreateUserAsync(user);

            Results.Created("", "Processed");
        });

        app.MapPut("/v1/user", async (IUserService service, User user) =>
        {
            await service.UpdateUserAsync(user);

            Results.Ok();
        });

        app.MapPut("/v1/user/active", async (IUserService service, ActiveUserRecord userRecord) =>
        {
            await service.ActivateUserAsync(userRecord);

            Results.Ok();
        });
    }
}
