using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IApplicantRepository
{
    public Task<bool> AddApplicantReference(ApplicantReference applicant);
    public Task<ApplicantReference?> GetApplicantReferenceById(Guid userId);

    public Task<ApplicantReference?> RetrieveApplicantReferenceById(Guid userId,
        bool includeAdmissionsWithPreferences = false);

    public Task<bool> RemoveApplicantReference(ApplicantReference applicant);
}