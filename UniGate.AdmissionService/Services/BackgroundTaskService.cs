using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Models;

namespace UniGateAPI.Services;

public class BackgroundTaskService(
    IApplicantRepository applicantRepository,
    IManagerRepository managerRepository,
    ApplicationDbContext dbContext)
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

    public async Task UpdateApplicantDocumentType(Guid userId, Guid documentTypeId)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReference(userId);

        if (foundApplicant == null)
            throw new InvalidOperationException("Tried to update a non-existing applicant");

        foundApplicant.TypeIdOfUploadedEducationDocument = documentTypeId;

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateManagerData(Guid userId, bool isChief, string fullName)
    {
        var foundManager = await managerRepository.RetrieveManagerReference(userId);

        if (foundManager == null)
        {
            await managerRepository.AddManagerReference(new ManagerReference
            {
                UserId = userId,
                IsChief = isChief,
                FullName = fullName
            });
        }
        else
        {
            foundManager.FullName = fullName;
            foundManager.IsChief = isChief;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveManager(Guid userId)
    {
        var foundManager = await managerRepository.RetrieveManagerReference(userId);

        if (foundManager == null)
            throw new InvalidOperationException("Tried to delete a non-existing manager");

        await managerRepository.RemoveManagerReference(foundManager);
    }
}