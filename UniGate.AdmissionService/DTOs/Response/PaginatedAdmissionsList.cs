namespace UniGateAPI.DTOs.Response;

public class PaginatedAdmissionsList
{
    public List<AdmissionDto> Admissions { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}