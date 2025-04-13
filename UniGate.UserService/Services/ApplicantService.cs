using Microsoft.AspNetCore.Identity;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.UserService.Data;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Mappers;
using UniGate.UserService.Models;

namespace UniGate.UserService.Services;

public class ApplicantService(
    IUserService userService,
    IApplicantRepository applicantRepository,
    UserManager<User> userManager,
    ITokenService tokenService,
    ApplicationDbContext dbContext) : IApplicantService
{
    public async Task<Result<TokenDto>> RegisterApplicant(RegisterApplicantDto applicant)
    {
        var createdUserResult = await userService.CreateUser(applicant.ToRegisterUserDto());

        if (createdUserResult.Code != HttpCode.Ok || createdUserResult.Data == null)
            return new Result<TokenDto>
            {
                Code = createdUserResult.Code,
                Message = createdUserResult.Message
            };

        await applicantRepository.AddApplicant(applicant.ToApplicant(createdUserResult.Data.Id));

        await userManager.AddToRoleAsync(createdUserResult.Data, "Applicant");

        var userRoles = await userManager.GetRolesAsync(createdUserResult.Data);

        return new Result<TokenDto>
            { Data = await tokenService.GenerateTokens(createdUserResult.Data.Id, userRoles.ToList()) };
    }

    public async Task<Result<ApplicantDto>> GetApplicantById(Guid userId)
    {
        var foundApplicant = await applicantRepository.GetApplicant(userId);

        return foundApplicant == null
            ? new Result<ApplicantDto>
            {
                Code = HttpCode.NotFound,
                Message = "Applicant not found"
            }
            : new Result<ApplicantDto>
            {
                Data = foundApplicant.ToDto()
            };
    }

    public async Task<Result> UpdateApplicant(Guid userId, UpdateApplicantDto applicant)
    {
        var foundApplicant = await applicantRepository.RetrieveApplicant(userId);

        if (foundApplicant == null)
            return
                new Result
                {
                    Code = HttpCode.NotFound,
                    Message = "Applicant not found"
                };

        foundApplicant.BirthDate = applicant.BirthDate;
        foundApplicant.Gender = applicant.Gender;
        foundApplicant.Citizenship = applicant.Citizenship;

        await dbContext.SaveChangesAsync();

        return new Result();
    }
}