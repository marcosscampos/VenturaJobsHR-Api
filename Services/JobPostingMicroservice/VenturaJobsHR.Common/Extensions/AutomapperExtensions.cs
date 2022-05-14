using VenturaJobsHR.Common.Mapping;

namespace VenturaJobsHR.Common.Extensions;

public static class AutomapperExtensions
{
    public static T ProjectedAs<T>(this object obj) where T : class
    {
        if (obj is null) return null;
        return MapperFactory.Mapper.Map<T>(obj);
    }

    public static T[] ProjectedAsCollection<T>(this object obj) where T : class
    {
        if (obj is null) return new T[0];
        return MapperFactory.Mapper.Map<IEnumerable<T>>(obj)?.ToArray() ?? new T[0];
    }
}
