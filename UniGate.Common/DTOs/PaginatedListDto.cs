namespace UniGate.Common.DTOs;

public class PaginatedListDto<T>
{
    public List<T> Items { get; set; }
    public PaginationDto Pagination { get; set; }
}