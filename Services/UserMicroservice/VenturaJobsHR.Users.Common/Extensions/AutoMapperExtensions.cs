using VenturaJobsHR.Users.Common.Mappings;

namespace VenturaJobsHR.Users.Common.Extensions;

public static class AutomapperExtensions
{
    public static T ProjectedAs<T>(this object obj) where T : class
    {
        if (obj is null) return null;
        return MapperFactory.Mapper.Map<T>(obj);
    }
}
