using VenturaJobsHR.Users.Domain.Models;
using VenturaJobsHR.Users.Domain.Seedwork.Repositories;

namespace VenturaJobsHR.Users.Domain.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
}
