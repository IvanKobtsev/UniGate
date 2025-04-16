using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IApplicantRepository
{
    public Task<bool> AddApplicantReference(ApplicantReference applicant);
    public Task<ApplicantReference?> GetApplicantReference(Guid userId);

    public Task<ApplicantReference?> RetrieveApplicantReference(Guid userId,
        bool includeAdmissionsWithPreferences = false);

    public Task<bool> RemoveApplicantReference(ApplicantReference applicant);
}