using MongoDB.Driver;
using VenturaJobsHR.Users.Domain.Abstractions.Repositories;
using VenturaJobsHR.Users.Domain.Models;
using VenturaJobsHR.Users.Repository.Context;

namespace VenturaJobsHR.Users.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IMongoContext context) : base(context)
    {

    }

    public async Task<User> GetUserByFireBaseToken(string firebaseId)
    {
        var user = await Collection.Find(x => x.FirebaseId == firebaseId).FirstOrDefaultAsync();
        return user;
    }
}
