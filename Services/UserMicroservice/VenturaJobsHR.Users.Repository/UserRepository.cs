using MongoDB.Driver;
using VenturaJobsHR.Users.CrossCutting.Enums;
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
        => await Collection.Find(x => x.FirebaseId == firebaseId).FirstOrDefaultAsync();
    
    public async Task<List<User>> GetUsersByTypeCompany()
        => await Collection.Find(x => x.UserType == UserTypeEnum.Company).ToListAsync();
}
