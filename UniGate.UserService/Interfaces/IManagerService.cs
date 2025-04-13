using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.DTOs.Responses;

namespace UniGate.UserService.Interfaces;

public interface IManagerService
{
    public Task<Result<CreateManagerResponseDto>> CreateManager(RegisterUserDto registerUserDto, bool isChief = false);
    public Task<Result<ManagersPagedListDto>> GetPaginatedManagers(int currentPage, int pageSize, bool chiefsOnly);
}