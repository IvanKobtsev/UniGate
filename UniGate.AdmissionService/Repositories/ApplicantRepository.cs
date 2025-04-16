using Microsoft.EntityFrameworkCore;
using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Models;

namespace UniGateAPI.Repositories;

public class ApplicantRepository(ApplicationDbContext dbContext) : IApplicantRepository
{
    public async Task<ApplicantReference?> GetApplicantReferenceById(Guid userId)
    {
        return await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<ApplicantReference?> RetrieveApplicantReferenceById(Guid userId,
        bool includeAdmissionsWithPreferences = false)
    {
        if (includeAdmissionsWithPreferences)
            return await dbContext.Applicants
                .Include(a => a.Admissions).ThenInclude(ad => ad.ProgramPreferences)
                .AsSplitQuery().AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId);

        return await dbContext.Applicants.FindAsync(userId);
    }

    public async Task<bool> RemoveApplicantReference(ApplicantReference applicant)
    {
        var existingApplicant =
            await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == applicant.UserId);

        if (existingApplicant == null) return false;

        dbContext.Applicants.Remove(existingApplicant);
        return true;
    }

    public async Task<bool> AddApplicantReference(ApplicantReference applicant)
    {
        var existingApplicant =
            await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == applicant.UserId);

        if (existingApplicant != null) return false;

        await dbContext.Applicants.AddAsync(applicant);
        return true;
    }
}