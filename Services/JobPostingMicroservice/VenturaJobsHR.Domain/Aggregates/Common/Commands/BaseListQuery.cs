using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VenturaJobsHR.CrossCutting.Pagination;

namespace VenturaJobsHR.Domain.Aggregates.Common.Commands;

public abstract class BaseListQuery<T> : BaseCommand
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
