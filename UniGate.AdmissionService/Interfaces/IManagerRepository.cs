using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IManagerRepository
{
    public Task<bool> AddManagerReference(ManagerReference manager);
    public Task<ManagerReference?> RetrieveManagerReference(Guid userId);
    public Task<ManagerReference?> GetManagerReference(Guid userId);
    public Task<bool> RemoveManagerReference(ManagerReference manager);
}