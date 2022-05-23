using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VenturaJobsHR.Bff.CrossCutting.Pagination;

namespace VenturaJobsHR.Bff.Domain.Seedwork.Query;

public abstract class BaseListQuery<T>
{
    public BaseListQuery()
    {
        Page = 1;
        Size = 15;
    }

    [FromQuery]
    public int Page { get; set; }

    [FromQuery]
    public int Size { get; set; }

    [JsonIgnore]
    public Page Pagination
    {
        get => new(rowsPerPage: Size, currentPage: Page);
    }
}
