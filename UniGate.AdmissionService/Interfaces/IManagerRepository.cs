using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IManagerRepository
{
    public Task<bool> AddManagerReference(ManagerReference manager);
    public Task<ManagerReference?> RetrieveManagerReferenceById(Guid userId);
    public Task<ManagerReference?> GetManagerReferenceById(Guid userId);
    public Task<bool> RemoveManagerReference(ManagerReference manager);
}