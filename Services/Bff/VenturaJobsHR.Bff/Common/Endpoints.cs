using VenturaJobsHR.Bff.Domain.Seedwork.Query;

namespace VenturaJobsHR.Bff.Common;

public static class Endpoints
{
    public const string JobEndpoint = "v1/jobs";
    public const string UserEndpoint = "v1/users";

    public static string JobEndpointWithQuery(SearchJobsQuery query)
    {
        var url = JobEndpoint;

        url += $"?page={query.Page}&size={query.Size}";

        if (query.Salary != 0 && query.FinalDate == DateTime.MinValue)
            url += $"&salary={query.Salary}";

        if (query.FinalDate != DateTime.MinValue && query.Salary == 0)
            url += $"&finalDate={query.FinalDate}";

        if (query.FinalDate != DateTime.MinValue && query.Salary != 0)
            url += $"&salary={query.Salary}&finalDate={query.FinalDate}";

        return url;
    }
}
