using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace VenturaJobsHR.Users.Common.Docs;

public static class OpenApiConfiguration
{
    public static void Configure(AspNetCoreOpenApiDocumentGeneratorSettings configure, string version)
    {
        configure.DocumentName = "Ventura Jobs HR";
        configure.ApiGroupNames = new[] { version };

        configure.PostProcess = document =>
        {
            document.Info.Title = "Ventura Jobs HR";
            document.Info.Description = "Microserviço responsável pela gestão de usuários.";
            document.Info.Version = version;
        };

        configure.AddSecurity("Bearer", new()
        {
            Name = "Authorization",
            Type = OpenApiSecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = @"JWT Authorization header using the Bearer scheme. <br/>
                      Enter 'Bearer' [space] and then your token in the text input below. <br/>
                      Example: 'Bearer 12345abcdef'"
        });

        configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
    }
}