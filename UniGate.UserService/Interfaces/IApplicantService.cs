using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;

namespace UniGate.UserService.Interfaces;

public interface IApplicantService
{
    public Task<Result<TokenDto>> RegisterApplicant(RegisterApplicantDto applicant);
    public Task<Result<ApplicantDto>> GetApplicantById(Guid id);
    public Task<Result> UpdateApplicant(Guid id, UpdateApplicantDto applicant);
}