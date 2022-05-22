using MongoDB.Bson;
using VenturaJobsHR.Users.Application.Factories;
using VenturaJobsHR.Users.Application.Records.User;
using VenturaJobsHR.Users.Application.Services.Interface;
using VenturaJobsHR.Users.Common.Exceptions;
using VenturaJobsHR.Users.Domain.Abstractions.Repositories;
using VenturaJobsHR.Users.Domain.Models;

namespace VenturaJobsHR.Users.Application.Services.Concrete;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task ActivateUserAsync(ActiveUserRecord userRecord)
    {
        if (!ObjectId.TryParse(userRecord.Id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var user = await _userRepository.GetByIdAsync(userRecord.Id);
        if (user is null)
            throw new NotFoundException($"User not found with ID #{userRecord.Id}");

        user.Active = userRecord.Active;

        await _userRepository.UpdateAsync(user);
    }

    public async Task CreateUserAsync(CreateUserRecord user)
    {
        var userCreated = UserFactory.CreateUser(user);
        await _userRepository.CreateAsync(userCreated);
    }

    public async Task<User> GetUserBy(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException($"User not found with ID #{id}");

        return user;
    }

    public async Task<IList<User>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.ToList();
    }

    public async Task UpdateUserAsync(UpdateUserRecord user)
    {
        if (!ObjectId.TryParse(user.Id, out _))
            throw new InvalidEntityIdProvidedException("Try with a valid ID.");

        var userToUpdate = await _userRepository.GetByIdAsync(user.Id);

        if (user is null)
            throw new NotFoundException($"User not found with ID #{user.Id}");

        UserFactory.UpdateUser(user, userToUpdate);

        await _userRepository.UpdateAsync(userToUpdate);
    }
}
