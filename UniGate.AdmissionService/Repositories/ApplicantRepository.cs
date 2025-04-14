using Microsoft.EntityFrameworkCore;
using UniGate.Common.Extensions;
using UniGate.Common.Utilities;
using UniGateAPI.Data;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Enums;
using UniGateAPI.Interfaces;
using UniGateAPI.Mappers;
using UniGateAPI.Models;

namespace UniGateAPI.Repositories;

public class ApplicantRepository(ApplicationDbContext dbContext) : IApplicantRepository
{
    public async Task<ApplicantReference?> GetApplicantReference(Guid userId)
    {
        return await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<ApplicantReference?> RetrieveApplicantReference(Guid userId)
    {
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

    public async Task<bool> AddAdmission(Admission admission)
    {
        var existingAdmission = await dbContext.Admissions
            .FirstOrDefaultAsync(a =>
                a.ApplicantId == admission.ApplicantId && a.AdmissionType == admission.AdmissionType);

        if (existingAdmission != null) return false;

        await dbContext.Admissions.AddAsync(admission);

        return true;
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

    public async Task<bool> AddApplicantReference(ApplicantReference applicant)
    {
        var existingApplicant =
            await dbContext.Applicants.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == applicant.UserId);

        if (existingApplicant != null) return false;

        await dbContext.Applicants.AddAsync(applicant);
        return true;
    }

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

    public async Task<Admission?> RetrieveAdmissionById(Guid admissionId)
    {
        return await dbContext.Admissions.FindAsync(admissionId);
    }
}