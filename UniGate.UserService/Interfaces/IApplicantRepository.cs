using UniGate.UserService.Models;

namespace UniGate.UserService.Interfaces;

public interface IApplicantRepository
{
    public Task AddApplicant(Applicant applicantDto);
    public Task<Applicant?> GetApplicant(Guid userId);
    public Task<Applicant?> RetrieveApplicant(Guid userId);
    public void DeleteApplicant(Applicant applicant);
}