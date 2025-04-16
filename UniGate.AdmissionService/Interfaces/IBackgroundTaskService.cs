namespace UniGateAPI.Interfaces;

public interface IBackgroundTaskService
{
    public Task UpdateApplicantName(Guid userId, string fullName);
    public Task RemoveApplicant(Guid userId);
    public Task UpdateApplicantDocumentType(Guid userId, Guid documentTypeId);
    public Task UpdateManagerData(Guid userId, bool isChief, string fullName);
    public Task RemoveManager(Guid userId);
}