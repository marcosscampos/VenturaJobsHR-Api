namespace VenturaJobsHR.Domain.SeedWork.Settings;

public interface IDbSettings
{
    string ConnectionStringMongoDb { get; set; }
    string DatabaseName { get; set; }
}
