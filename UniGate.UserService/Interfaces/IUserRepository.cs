using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Models;

namespace UniGate.UserService.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUser(Guid userId);
    public Task<User?> RetrieveUser(Guid userId);

    public Task<PaginatedList<ProfileDto>>
        GetPagedUsersFilteredByRoles(int pageIndex, int pageSize, List<string> roles);
}