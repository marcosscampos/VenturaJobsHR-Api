using VenturaJobsHR.Bff.CrossCutting.Settings.Interfaces;

namespace VenturaJobsHR.Bff.CrossCutting.Settings.Concrete;

public class ServiceSettings : IServiceSettings
{
    public string UrlApiJobsV1 { get; set; }
    public string UrlApiUsersV1 { get; set; }
    public int Timeout { get; set; }
}
