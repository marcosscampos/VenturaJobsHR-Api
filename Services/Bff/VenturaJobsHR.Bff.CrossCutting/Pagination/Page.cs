namespace VenturaJobsHR.Bff.CrossCutting.Pagination;

public class Page
{
    public Page()
    {
        Length = 15;
        StartIndex = 0;
        CurrentPage = 1;
    }

    public Page(int rowsPerPage, int currentPage = 1)
    {
        Length = rowsPerPage;
        CurrentPage = currentPage;
        StartIndex = --currentPage * Length;
    }

    public int CurrentPage { get; set; }
    public int Length { get; set; }
    public int StartIndex { get; set; }

    public static long PageCount(long total, int rowsPerPage)
    {
        long pageCount = total / rowsPerPage;

        if (total % rowsPerPage == 0) pageCount++;

        return pageCount;
    }
}
