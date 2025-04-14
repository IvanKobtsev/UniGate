namespace UniGate.Common.Utilities;

public class PaginatedList<T>
{
    public required List<T> Items { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int PagesCount => TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
}