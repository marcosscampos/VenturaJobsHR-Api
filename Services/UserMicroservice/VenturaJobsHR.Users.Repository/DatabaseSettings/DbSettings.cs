using VenturaJobsHR.Users.Domain.Seedwork.Settings;

namespace VenturaJobsHR.Users.Repository.DatabaseSettings;

public class DbSettings : IDbSettings
{
    public string ConnectionStringMongoDb { get; set; }
    public string DatabaseName { get; set; }
}
