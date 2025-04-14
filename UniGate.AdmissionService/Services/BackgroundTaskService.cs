using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Models;

namespace UniGateAPI.Services;

public class BackgroundTaskService(IApplicantRepository applicantRepository, ApplicationDbContext dbContext)
    : IBackgroundTaskService
{
    public async Task UpdateApplicantName(Guid userId, string fullName)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReference(userId);

        if (foundApplicant == null)
            await applicantRepository.AddApplicantReference(new ApplicantReference
            {
                UserId = userId,
                FullName = fullName
            });
        else
            foundApplicant.FullName = fullName;

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveApplicant(Guid userId)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReference(userId);

        if (foundApplicant == null)
            throw new InvalidOperationException("Tried to delete a non-existing applicant");

        await applicantRepository.RemoveApplicantReference(foundApplicant);
    }
}