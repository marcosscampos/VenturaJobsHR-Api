namespace VenturaJobsHR.Bff.CrossCutting.Settings.Interfaces;

public interface IServiceSettings
{
    string UrlApiJobsV1 { get; set; }
    string UrlApiUsersV1 { get; set; }
    int Timeout { get; set; }
}