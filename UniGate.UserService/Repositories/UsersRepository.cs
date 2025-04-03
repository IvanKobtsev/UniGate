using UniGate.Common.Exceptions;
using UniGate.UserService.Data;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;

namespace UniGate.UserService.Repositories;

public class UsersRepository(ApplicationDbContext context) : IUsersRepository
{
    public async Task<User> GetUser(string userId)
    {
        var foundUser = await context.Users.FindAsync(userId);

        if (foundUser == null) throw new NotFoundException($"User with ID {userId} not found.");

        return foundUser;
    }
}