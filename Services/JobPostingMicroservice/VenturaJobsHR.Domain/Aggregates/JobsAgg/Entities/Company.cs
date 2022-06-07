namespace VenturaJobsHR.Domain.Aggregates.JobsAgg.Entities;

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

    public void Update(string id, string uid, string name)
    {
        if (!Id.Equals(id))
            Id = id;

        if (!Uid.Equals(uid))
            Uid = uid;

        if (!Name.Equals(name))
            Name = name;
    }

    public string Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
}
