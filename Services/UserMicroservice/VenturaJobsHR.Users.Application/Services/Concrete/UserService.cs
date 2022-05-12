using VenturaJobsHR.Users.Application.Records;
using VenturaJobsHR.Users.Application.Services.Interface;
using VenturaJobsHR.Users.Domain.Abstractions.Repositories;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Application.Services.Concrete;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ActivateUserAsync(ActiveUserRecord userRecord)
    {
        var user = await _userRepository.GetByIdAsync(userRecord.Id);
        user.Active = userRecord.Active;

        await _userRepository.UpdateAsync(user);
    }

    public async Task CreateUserAsync(User user)
    {
        try
        {
            await _userRepository.CreateAsync(user);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<User> GetUserBy(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user;
    }

    public async Task<IList<User>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.ToList();
    }

    public async Task UpdateUserAsync(User user)
    {
        var userToUpdate = await _userRepository.GetByIdAsync(user.Id);

        await _userRepository.UpdateAsync(userToUpdate);
    }
}
