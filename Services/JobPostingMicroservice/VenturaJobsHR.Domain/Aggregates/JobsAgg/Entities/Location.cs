namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

public class Location
{
    public Location(string city, string state, string country)
    {
        City = city;
        State = state;
        Country = country;
    }

    public Location()
    {

    }

    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
