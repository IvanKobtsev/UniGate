using UniGate.Common.DTOs;
using UniGateAPI.DTOs.Common;

namespace UniGateAPI.DTOs.Response;

public class PaginatedAdmissionsList
{
    public List<AdmissionFullDto> Admissions { get; set; } = [];
    public required PaginationDto Pagination { get; set; }
}