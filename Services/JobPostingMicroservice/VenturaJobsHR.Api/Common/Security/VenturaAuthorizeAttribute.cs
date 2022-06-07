using Microsoft.AspNetCore.Mvc;

namespace VenturaJobsHR.Api.Common.Security;

public class VenturaAuthorizeAttribute : TypeFilterAttribute
{
    public VenturaAuthorizeAttribute(string role) : base(typeof(VenturaActionFilter))
    {
        Arguments = new object[] { role };
    }
}