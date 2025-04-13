namespace UniGate.Common.Utilities;

public class PaginatedList<T>
{
    public required List<T> Items { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int PagesCount { get; set; }
}