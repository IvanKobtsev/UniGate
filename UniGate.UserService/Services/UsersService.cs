using Microsoft.AspNetCore.Identity;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Enums;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Mappers;
using UniGate.UserService.Models;

namespace UniGate.UserService.Services;

public class UsersService(
    UserManager<User> userManager,
    ITokenService tokenService,
    ITokenStore tokenStore,
    IUsersRepository usersRepository)
    : IUsersService
{
    public async Task<Result<TokenDto>> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Patronymic = registerDto.Patronymic ?? string.Empty,
            PhoneNumber = registerDto.PhoneNumber ?? string.Empty,
            Role = Role.Applicant
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return new Result<TokenDto> { Code = HttpCode.BadRequest, Message = result.Errors.First().Description };

        var createdUser = await userManager.FindByEmailAsync(registerDto.Email);

        return createdUser == null
            ? new Result<TokenDto> { Code = HttpCode.BadRequest, Message = "Something went wrong while creating user" }
            : new Result<TokenDto> { Data = await tokenService.GenerateTokens(createdUser.Id, createdUser.Role) };
    }

    public async Task<Result> UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return new Result { Code = HttpCode.NotFound, Message = "User not found" };

        var result =
            await userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword,
                updatePasswordDto.NewPassword);

        return !result.Succeeded
            ? new Result { Code = HttpCode.BadRequest, Message = result.Errors.First().Description }
            : new Result();
    }

    public async Task<Result> UpdateProfile(string userId, UpdateProfileDto updateProfileDto)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return new Result { Code = HttpCode.NotFound, Message = "User not found" };

        user.UserName = updateProfileDto.Email;
        user.Email = updateProfileDto.Email;
        user.FirstName = updateProfileDto.FirstName;
        user.LastName = updateProfileDto.LastName;
        user.Patronymic = updateProfileDto.Patronymic;
        user.PhoneNumber = updateProfileDto.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        return !result.Succeeded
            ? new Result { Code = HttpCode.BadRequest, Message = result.Errors.First().Description }
            : new Result();
    }

    public async Task<Result<ProfileDto>> GetProfileDto(string userId)
    {
        var profileDto = (await usersRepository.GetUser(userId))?.ToDto();

        return profileDto == null
            ? new Result<ProfileDto> { Message = "User not found", Code = HttpCode.NotFound }
            : new Result<ProfileDto> { Data = profileDto };
    }

    public async Task<Result<TokenDto>> Login(LoginDto loginDto)
    {
        var foundUser = await userManager.FindByEmailAsync(loginDto.Email);

        if (foundUser == null)
            return new Result<TokenDto> { Message = "Invalid credentials", Code = HttpCode.BadRequest };

        if (!await userManager.CheckPasswordAsync(foundUser, loginDto.Password))
            return new Result<TokenDto> { Message = "Invalid credentials", Code = HttpCode.BadRequest };

        return new Result<TokenDto> { Data = await tokenService.GenerateTokens(foundUser.Id, foundUser.Role) };
    }

    public async Task<Result<TokenDto>> RefreshToken(string refreshToken)
    {
        var userId = await tokenStore.GetUserIdByRefreshToken(refreshToken);

        if (userId == null)
            return new Result<TokenDto> { Code = HttpCode.BadRequest, Message = "Invalid refresh token" };

        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return new Result<TokenDto> { Message = "Invalid refresh token", Code = HttpCode.NotFound };

        return new Result<TokenDto>
        {
            Data = new TokenDto
            {
                AccessToken = tokenService.GenerateAccessToken(user.Id, user.Role),
                RefreshToken = refreshToken
            }
        };
    }
}