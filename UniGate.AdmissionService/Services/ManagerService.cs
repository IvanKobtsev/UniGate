using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGateAPI.Data;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Interfaces;
using UniGateAPI.Mappers;
using UniGateAPI.Models;

namespace UniGateAPI.Services;

public class ManagerService(
    IManagerRepository managerRepository,
    IApplicantRepository applicantRepository,
    IAdmissionRepository admissionRepository,
    ApplicationDbContext dbContext)
    : IManagerService
{
    public async Task UpdateManager(Guid userId, string fullName, bool isChief)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicantReference(userId);

        if (foundApplicant == null)
            await managerRepository.AddManagerReference(new ManagerReference
            {
                UserId = userId,
                FullName = fullName,
                IsChief = isChief
            });
        else
            foundApplicant.FullName = fullName;

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveManager(Guid userId)
    {
        var foundManager = await managerRepository.RetrieveManagerReference(userId);

        if (foundManager == null)
            throw new InvalidOperationException("Tried to delete a non-existing manager");

        await managerRepository.RemoveManagerReference(foundManager);
    }

    public async Task<Result> AssignManagerToAdmission(Guid managerId, Guid admissionId)
    {
        var foundAdmission = await admissionRepository.RetrieveAdmissionById(admissionId);

        if (foundAdmission == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Admission not found"
            };

        var foundManager = await applicantRepository.RetrieveApplicantReference(managerId);

        if (foundManager == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Manager not found"
            };

        foundAdmission.ManagerId = managerId;
        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public async Task<Result<ManagerDto>> GetManagerProfile(Guid userId)
    {
        var foundManager = await managerRepository.GetManagerReference(userId);

        if (foundManager == null)
            return new Result<ManagerDto>
            {
                Code = HttpCode.NotFound,
                Message = "Manager reference not found"
            };

        return new Result<ManagerDto>
        {
            Data = foundManager.ToDto()
        };
    }

    public async Task<Result> AssignManagerToFaculty(Guid managerId, Guid facultyId)
    {
        var foundManager = await managerRepository.RetrieveManagerReference(managerId);

        if (foundManager == null)
            return new Result
            {
                Code = HttpCode.NotFound,
                Message = "Manager not found"
            };

        // var foundFaculty = await managerRepository.RetrieveFacultyById(facultyId);
        //
        // if (foundFaculty == null)
        //     return new Result
        //     {
        //         Code = HttpCode.NotFound,
        //         Message = "Faculty not found"
        //     };

        foundManager.AssignedFacultyId = facultyId;

        await dbContext.SaveChangesAsync();

        return new Result();
    }

    public bool CanManagerEditAdmission(ManagerReference manager, Admission admission)
    {
        return manager.IsChief ||
               (manager.AssignedFacultyId != null && manager.AssignedFacultyId == admission
                   .ProgramPreferences.FirstOrDefault(pp => pp.Priority == 1)
                   ?.FacultyIdOfChosenProgram) ||
               manager.UserId == admission.ManagerId;
    }
}