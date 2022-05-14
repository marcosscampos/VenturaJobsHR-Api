using VenturaJobsHR.Domain.Aggregates.Common.Settings;

namespace VenturaJobsHR.Repository.DatabaseSettings;

public class DbSettings : IDbSettings
{
    public string ConnectionStringMongoDb { get; set; }
    public string DatabaseName {get; set;}
}
