namespace VenturaJobsHR.Bff.CrossCutting.Pagination;

public class Pagination<T> where T : class
{
    public Pagination()
    {
        Data = new List<T>();
    }

    public Pagination(IEnumerable<T> data, int currentPage, int pageSize, long items)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        Total = items;
        Data = data.ToList();
    }

    public long Total { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public List<T> Data { get; set; }
}
