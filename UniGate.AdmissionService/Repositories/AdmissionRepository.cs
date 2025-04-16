using Microsoft.EntityFrameworkCore;
using UniGate.Common.Enums;
using UniGate.Common.Extensions;
using UniGate.Common.Utilities;
using UniGateAPI.Data;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Enums;
using UniGateAPI.Interfaces;
using UniGateAPI.Mappers;
using UniGateAPI.Models;

namespace UniGateAPI.Repositories;

public class AdmissionRepository(ApplicationDbContext dbContext) : IAdmissionRepository
{
    public async Task<PaginatedList<AdmissionFullDto>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus,
        bool onlyNotTaken,
        bool onlyMine, Sorting sorting,
        int page, int pageSize)
    {
        var admissions = dbContext.Admissions.AsNoTracking();

        switch (sorting)
        {
            default:
            case Sorting.DateDesc:
                admissions =
                    admissions.OrderByDescending(a => a.CreateTime);
                break;
            case Sorting.DateAsc:
                admissions =
                    admissions.OrderBy(a => a.CreateTime);
                break;
        }

        var totalCount = await admissions.CountAsync();

        var result = await admissions.Include(a => a.ApplicantReference).Include(a => a.ProgramPreferences)
            .Paginate(page, pageSize)
            .ToListAsync();

        return new PaginatedList<AdmissionFullDto>
        {
            Items = result.ToFullDtos(),
            PageSize = pageSize,
            TotalCount = totalCount,
            PageIndex = page
        };
    }


    public async Task<Admission?> GetAdmissionById(Guid admissionId, bool includeChosenPrograms = false)
    {
        if (includeChosenPrograms)
            return await dbContext.Admissions.Include(a => a.ProgramPreferences)
                .FirstOrDefaultAsync(a => a.Id == admissionId);

        return await dbContext.Admissions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == admissionId);
    }

    public async Task<Admission?> RetrieveAdmissionById(Guid admissionId,
        bool includeChosenProgramsAndApplicantRef = false)
    {
        if (includeChosenProgramsAndApplicantRef)
            return await dbContext.Admissions.Include(a => a.ProgramPreferences)
                .Include(a => a.ApplicantReference)
                .AsSplitQuery()
                .FirstOrDefaultAsync(a => a.Id == admissionId);

        return await dbContext.Admissions.FindAsync(admissionId);
    }

    public async Task<Guid?> AddAdmission(Admission admission)
    {
        var existingAdmission = await dbContext.Admissions
            .FirstOrDefaultAsync(a =>
                a.ApplicantId == admission.ApplicantId && a.AdmissionType == admission.AdmissionType);

        if (existingAdmission != null) return null;

        await dbContext.Admissions.AddAsync(admission);

        return admission.Id;
    }


    public async Task<bool> AddProgramPreference(ProgramPreference programPreference)
    {
        var existingProgramPreference = await dbContext.ProgramPreferences
            .FirstOrDefaultAsync(a =>
                a.AdmissionId == programPreference.AdmissionId &&
                a.ChosenProgramId == programPreference.ChosenProgramId);

        if (existingProgramPreference != null) return false;

        await dbContext.ProgramPreferences.AddAsync(programPreference);

        return true;
    }

    public async Task<bool> RemoveProgramPreference(ProgramPreference programPreference)
    {
        var existingProgramPreference = await dbContext.ProgramPreferences
            .FirstOrDefaultAsync(a =>
                a.AdmissionId == programPreference.AdmissionId &&
                a.ChosenProgramId == programPreference.ChosenProgramId);

        if (existingProgramPreference == null) return false;

        dbContext.ProgramPreferences.Remove(existingProgramPreference);

        return true;
    }

    public async Task<ProgramPreference?> RetrieveProgramPreferenceById(Guid programPreferenceId,
        bool includeAdmission = false)
    {
        if (includeAdmission)
            return await dbContext.ProgramPreferences.Include(a => a.Admission)
                .ThenInclude(ad => ad.ApplicantReference).AsSplitQuery()
                .FirstOrDefaultAsync(a => a.Id == programPreferenceId);

        return await dbContext.ProgramPreferences.FindAsync(programPreferenceId);
    }
}