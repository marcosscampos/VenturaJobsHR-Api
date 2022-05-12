using VenturaJobsHR.Users.Application.Records;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Application.Services.Interface;

public interface IUserService
{
    Task<IList<User>> GetUsersAsync();
    Task<User> GetUserBy(string id);
    Task UpdateUserAsync(User user);
    Task CreateUserAsync(User user);
    Task ActivateUserAsync(ActiveUserRecord userRecord);
}
