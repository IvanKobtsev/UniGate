using UniGate.Common.DTOs;
using UniGate.UserService.DTOs.Common;

namespace UniGate.UserService.DTOs.Responses;

public class ManagersPagedListDto
{
    public List<ProfileDto> Managers { get; set; }
    public PaginationDto Pagination { get; set; }
}