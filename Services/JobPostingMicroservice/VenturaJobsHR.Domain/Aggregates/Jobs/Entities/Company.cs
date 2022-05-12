namespace VenturaJobsHR.Domain.Aggregates.Jobs.Entities;

public class Company
{
    public Company(string id, string uid, string name)
    {
        Id = id;
        Uid = uid;
        Name = name;
    }

    public Company()
    {

    }

    public string Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
}
