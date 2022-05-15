using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using VenturaJobsHR.Api.Docs.Extensions;

namespace VenturaJobsHR.Api.Seedwork.Swagger.Filters;

public class SwaggerIgnoreFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties.Count == 0)
            return;

        const BindingFlags bindingFlags = BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;
        var memberList = context.Type
                            .GetFields(bindingFlags).Cast<MemberInfo>()
                            .Concat(context.Type
                            .GetProperties(bindingFlags));

        var excludedList = memberList.Where(m =>
                                            m.GetCustomAttribute<JsonIgnoreAttribute>()
                                            != null)
                                     .Select(m =>
                                         (m.GetCustomAttribute<JsonPropertyAttribute>()
                                          ?.PropertyName
                                          ?? m.Name.ToCamelCase()));

        foreach (var excludedName in excludedList)
        {
            if (schema.Properties.ContainsKey(excludedName))
                schema.Properties.Remove(excludedName);
        }
    }
}
