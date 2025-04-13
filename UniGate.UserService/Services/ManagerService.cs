using Microsoft.AspNetCore.Identity;
using UniGate.Common.DTOs;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.DTOs.Responses;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;

namespace UniGate.UserService.Services;

public class ManagerService(IUserService userService, IUserRepository userRepository, UserManager<User> userManager)
    : IManagerService
{
    public async Task<Result<CreateManagerResponseDto>> CreateManager(RegisterUserDto registerUserDto,
        bool isChief = false)
    {
        var createdUserResult = await userService.CreateUser(registerUserDto);

        if (createdUserResult.IsFailed || createdUserResult.Data == null)
            return new Result<CreateManagerResponseDto>
            {
                Code = createdUserResult.Code,
                Message = createdUserResult.Message
            };

        var role = "Manager";

        if (isChief)
            role = "ChiefManager";

        var result = await userManager.AddToRoleAsync(createdUserResult.Data, role);

        if (!result.Succeeded)
            return new Result<CreateManagerResponseDto>
            {
                Code = HttpCode.BadRequest,
                Message = result.Errors.FirstOrDefault()?.Description
            };

        return new Result<CreateManagerResponseDto>
        {
            Data = new CreateManagerResponseDto { CreatedManagerId = createdUserResult.Data.Id }
        };
    }

    public async Task<Result<ManagersPagedListDto>> GetPaginatedManagers(int currentPage, int pageSize, bool chiefsOnly)
    {
        List<string> roles = ["ChiefManager"];

        if (!chiefsOnly) roles.Add("Manager");

        var paginatedResult = await userRepository.GetPagedUsersFilteredByRoles(currentPage, pageSize, roles);

        return new Result<ManagersPagedListDto>
        {
            Data = new ManagersPagedListDto
            {
                Managers = paginatedResult.Items,
                Pagination = new PaginationDto
                {
                    Count = paginatedResult.PagesCount,
                    Current = paginatedResult.PageIndex,
                    Size = paginatedResult.PageSize
                }
            }
        };
    }
}