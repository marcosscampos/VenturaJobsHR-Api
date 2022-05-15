using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VenturaJobsHR.Api.Seedwork.Swagger.Filters;

public class JsonIgnoreQueryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.ParameterDescriptions
                .Where(d => true).ToList()
                .ForEach(param =>
                {
                    var toIgnore =
                       ((DefaultModelMetadata)param.ModelMetadata)?
                       .Attributes.PropertyAttributes
                       ?.Any(x => x is JsonIgnoreAttribute || x.ToString().ToLower().Contains("jsonignore")) ?? false;

                    toIgnore |= param.Name.Contains('.') && context.ApiDescription.HttpMethod == "GET";

                    var toRemove = operation.Parameters
                        .FirstOrDefault(p => p.Name.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (toIgnore)
                        operation.Parameters.Remove(toRemove);
                });
    }
}
