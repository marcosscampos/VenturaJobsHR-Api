using AutoMapper;
using System.Reflection;

namespace VenturaJobsHR.Users.Common.Mappings;

public static class MapperFactory
{
    static Mapper _mapper;
    public static Mapper Mapper { get { return _mapper ?? throw new Exception("Mapper Not Initialized"); } }

    public static void Setup()
    {
        var nmspace = Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;

        var profiles = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(p => p.FullName.Contains("Domain"))
                        .SelectMany(p => p.GetTypes())
                        .Where(p => p.BaseType == typeof(Profile))
                        .ToList();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

            foreach (var profile in profiles)
            {
                cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        });

        _mapper = new Mapper(config);
    }
}
