using VenturaJobsHR.Bff.Domain.Seedwork.Query;

namespace VenturaJobsHR.Bff.Common;

public static class Endpoints
{
    public const string JobEndpoint = "v1/jobs";
    public const string UserEndpoint = "v1/users";
    public const string ApplicationEndpoint = "v1/jobApplications";

    public static string JobEndpointWithQuery(SearchJobsQuery query)
    {
        var url = JobEndpoint;

        url += $"?page={query.Page}&size={query.Size}";

        if (query.Salary != 0 && query.DeadLine == DateTime.MinValue && query.OccupationArea == 0)
            url += $"&salary={query.Salary}";

        if (query.DeadLine != DateTime.MinValue && query.Salary == 0 && query.OccupationArea == 0)
            url += $"&finalDate={query.DeadLine}";

        if (query.DeadLine == DateTime.MinValue && query.Salary == 0 && query.OccupationArea != 0)
            url += $"&occupationArea={query.OccupationArea}";

        if (query.DeadLine != DateTime.MinValue && query.Salary != 0 && query.OccupationArea != 0)
            url += $"&salary={query.Salary}&finalDate={query.DeadLine}&occupationArea={query.OccupationArea}";

        return url;
    }

    public static string JobCompanyEndpointWithQuery(SearchJobsQuery query)
    {
        var url = JobEndpoint + "/company";

        url += $"?page={query.Page}&size={query.Size}";

        if (query.Salary != 0 && query.DeadLine == DateTime.MinValue && query.OccupationArea == 0)
            url += $"&salary={query.Salary}";

        if (query.DeadLine != DateTime.MinValue && query.Salary == 0 && query.OccupationArea == 0)
            url += $"&finalDate={query.DeadLine}";

        if (query.DeadLine == DateTime.MinValue && query.Salary == 0 && query.OccupationArea != 0)
            url += $"&occupationArea={query.OccupationArea}";

        if (query.DeadLine != DateTime.MinValue && query.Salary != 0 && query.OccupationArea != 0)
            url += $"&salary={query.Salary}&finalDate={query.DeadLine}&occupationArea={query.OccupationArea}";

        return url;
    }
}
