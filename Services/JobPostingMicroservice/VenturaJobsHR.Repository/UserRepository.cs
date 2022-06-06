using MongoDB.Driver;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Entities;
using VenturaJobsHR.Domain.Aggregates.UserAgg.Repositories;
using VenturaJobsHR.Repository.Context;

namespace VenturaJobsHR.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IMongoContext context) : base(context) { }

    public async Task<User> GetUserByFirebaseId(string firebaseId)
        => await Collection.Find(x => x.FirebaseId == firebaseId).SingleOrDefaultAsync();
}
