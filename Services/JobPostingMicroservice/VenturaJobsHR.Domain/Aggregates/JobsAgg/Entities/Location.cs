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
    
    public void Update(string city, string state, string country)
    {
        if (!City.Equals(city))
            City = city;

        if (!State.Equals(state))
            State = state;

        if (!Country.Equals(country))
            Country = country;
    }

    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
