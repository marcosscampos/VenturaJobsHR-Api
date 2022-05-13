using VenturaJobsHR.Users.Application.Records.User;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Application.Services.Interface;

public interface IUserService
{
    Task<IList<User>> GetUsersAsync();
    Task<User> GetUserBy(string id);
    Task UpdateUserAsync(UpdateUserRecord user);
    Task CreateUserAsync(CreateUserRecord user);
    Task ActivateUserAsync(ActiveUserRecord userRecord);
}
