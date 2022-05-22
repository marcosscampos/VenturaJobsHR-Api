using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using VenturaJobsHR.Bff.Seedwork.Swagger.Filters;

namespace VenturaJobsHR.Bff.Seedwork.Swagger;

public static class SwaggerConfiguration
{
    public static void Configure(SwaggerGenOptions configure, string version)
    {
        configure.CustomSchemaIds(type => type.ToString());
        ConfigureSchemas(configure);
        configure.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        configure.OperationFilter<JsonIgnoreQueryOperationFilter>();
        configure.OperationFilter<SwaggerExcludeFilter>();
        configure.SchemaFilter<SwaggerIgnoreFilter>();
        configure.ExampleFilters();

        configure.EnableAnnotations();

        configure.SwaggerDoc(version, new OpenApiInfo()
        {
            Version = version,
            Title = "Ventura Jobs HR",
            Description = "BFF responsável por unificar todos os microserviços para poder ser consumido no front end",

            Contact = new OpenApiContact
            {
                Name = "Marcos Vinícius Simões Campos",
                Email = "marcos.scampos@al.infnet.edu.br",
                Url = new Uri("https://ventura-jobs-hr.netlify.app")
            }
        });


        configure.AddSecurityDefinition("Bearer", new()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = @"JWT Authorization header using the Bearer scheme. <br/>
                      Enter 'Bearer' [space] and then your token in the text input below. <br/>
                      Example: 'Bearer 12345abcdef'"
        });

        configure.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme{
                    Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        configure.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }

    private static void ConfigureSchemas(SwaggerGenOptions options)
    {
        options.DescribeAllParametersInCamelCase();
    }
}
