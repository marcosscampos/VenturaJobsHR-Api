namespace VenturaJobsHR.Domain.Aggregates.Common.Settings;

public interface IDbSettings
{
    string ConnectionStringMongoDb { get; set; }
    string DatabaseName { get; set; }
}
