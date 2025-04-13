using Microsoft.EntityFrameworkCore;
using UniGate.UserService.Data;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;

namespace UniGate.UserService.Repositories;

public class ApplicantRepository(ApplicationDbContext dbContext) : IApplicantRepository
{
    public async Task AddApplicant(Applicant applicant)
    {
        await dbContext.Applicants.AddAsync(applicant);

        await dbContext.SaveChangesAsync();
    }

    public async Task<Applicant?> GetApplicant(Guid userId)
    {
        return await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Applicant?> RetrieveApplicant(Guid userId)
    {
        return await dbContext.Applicants.FindAsync(userId);
    }

    public void DeleteApplicant(Applicant applicant)
    {
        dbContext.Applicants.Remove(applicant);
    }
}