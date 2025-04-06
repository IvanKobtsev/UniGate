using UniGate.UserService.Models;

namespace UniGate.UserService.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUser(string userId);
}