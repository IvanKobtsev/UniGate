using Microsoft.EntityFrameworkCore;
using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Models;

namespace UniGateAPI.Repositories;

public class ManagerRepository(ApplicationDbContext dbContext) : IManagerRepository
{
    public async Task<bool> AddManagerReference(ManagerReference manager)
    {
        var existingManager = await dbContext.Managers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserId == manager.UserId);

        if (existingManager != null)
            return false;

        await dbContext.Managers.AddAsync(manager);

        return true;
    }

    public async Task<ManagerReference?> RetrieveManagerReference(Guid userId)
    {
        return await dbContext.Managers.FindAsync(userId);
    }

    public async Task<ManagerReference?> GetManagerReference(Guid userId)
    {
        return await dbContext.Managers.AsNoTracking().Include(m => m.AssignedAdmissions)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<bool> RemoveManagerReference(ManagerReference manager)
    {
        var existingManager = await dbContext.Applicants.FindAsync(manager.UserId);
        if (existingManager == null) return false;

        dbContext.Applicants.Remove(existingManager);
        return true;
    }
}