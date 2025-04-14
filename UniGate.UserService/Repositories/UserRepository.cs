using Microsoft.EntityFrameworkCore;
using UniGate.Common.Extensions;
using UniGate.Common.Utilities;
using UniGate.UserService.Data;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;

namespace UniGate.UserService.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> RetrieveUser(Guid userId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUser(Guid userId)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<PaginatedList<ProfileDto>> GetPagedUsersFilteredByRoles(int currentPage, int pageSize,
        List<string> roles)
    {
        var roleIds = await context.Roles
            .Where(r => roles.Contains(r.Name!))
            .Select(r => r.Id).ToListAsync();

        var users = from u in context.Users
            join ur in context.UserRoles on u.Id equals ur.UserId
            join r in context.Roles on ur.RoleId equals r.Id
            where roleIds.Contains(r.Id)
            select new ProfileDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Patronymic = u.Patronymic,
                PhoneNumber = u.PhoneNumber,
                Roles = new List<string> { r.Name }
            };

        var totalCount = await users.CountAsync();

        var result = await users.Paginate(currentPage, pageSize).ToListAsync();

        return new PaginatedList<ProfileDto>
        {
            Items = result,
            PageIndex = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}