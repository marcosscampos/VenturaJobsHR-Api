using VenturaJobsHR.Users.Domain.Models;
using VenturaJobsHR.Users.Domain.Seedwork.Repositories;

namespace VenturaJobsHR.Users.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByFireBaseToken(string firebaseId);
    Task<List<User>> GetUsersByTypeCompany();
}
