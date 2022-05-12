using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using VenturaJobsHR.Users.Domain.Models;
using VenturaJobsHR.Users.Repository.Persistence;

namespace VenturaJobsHR.Users.Repository.Mappings;

public class ConfigurationDbMap : RepositoryMapBase
{
    public override void Configure()
    {
        BsonClassMap.RegisterClassMap<User>(x =>
        {
            x.AutoMap();
            x.SetIgnoreExtraElements(true);
            x.MapIdMember(x => x.Id);
            x.MapIdProperty(x => x.Id)
            .SetIdGenerator(StringObjectIdGenerator.Instance)
            .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
    }
}
