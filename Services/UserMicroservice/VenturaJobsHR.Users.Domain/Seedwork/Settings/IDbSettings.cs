namespace VenturaJobsHR.Users.Domain.Seedwork.Settings;

public interface IDbSettings
{
    string ConnectionStringMongoDb { get; set; }
    string DatabaseName { get; set; }
}
