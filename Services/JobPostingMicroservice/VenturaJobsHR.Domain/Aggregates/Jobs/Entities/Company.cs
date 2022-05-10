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
    //public string Id { get; set; }
    //public string Name { get; set; }
    //public Address Address { get; set; }
    //public string Phone { get; set; }
    //public string Email { get; set; }
    //public LegalRecord LegalRecord { get; set; }
    //public IList<Job> JobsList { get; set; }
}
