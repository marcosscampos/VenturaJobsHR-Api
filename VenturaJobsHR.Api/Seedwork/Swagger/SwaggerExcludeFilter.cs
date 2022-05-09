using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace VenturaJobsHR.Api.Seedwork.Swagger
{
    public class SwaggerExcludeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(x => x.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null));

            if (ignoredProperties.Any())
            {
                foreach (var property in ignoredProperties)
                {
                    operation.Parameters = operation.Parameters.Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture) && 
                    !p.In.Value.ToString().Equals("route", StringComparison.InvariantCultureIgnoreCase))).ToList();
                }
            }
        }
    }
}
