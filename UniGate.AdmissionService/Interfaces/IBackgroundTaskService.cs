namespace UniGateAPI.Interfaces;

public interface IBackgroundTaskService
{
    public Task UpdateApplicantName(Guid userId, string fullName);
    public Task RemoveApplicant(Guid userId);
}