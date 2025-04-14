using UniGate.Common.Utilities;
using UniGateAPI.DTOs.Common;

namespace UniGateAPI.Interfaces;

public interface IManagerService
{
    public Task<Result> AssignManagerToAdmission(Guid managerId, Guid admissionId);
    public Task UpdateManager(Guid userId, string fullName, bool isChief);
    public Task<Result<ManagerDto>> GetManagerProfile(Guid userId);
    public Task RemoveManager(Guid userId);
}