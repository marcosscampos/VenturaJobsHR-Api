using VenturaJobsHR.Domain.Aggregates.Common.Repositories;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Entities;

namespace VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByFirebaseId(string firebaseId);
    Task<List<User>> GetUsersByIdList(List<string> ids);
}
