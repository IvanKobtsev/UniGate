using UniGate.UserService.Data;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;

namespace UniGate.UserService.Repositories;

public class UsersRepository(ApplicationDbContext context) : IUsersRepository
{
    public async Task<User?> GetUser(string userId)
    {
        return await context.Users.FindAsync(userId);
    }
}