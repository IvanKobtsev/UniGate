using Microsoft.AspNetCore.Identity;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Mappers;
using UniGate.UserService.Models;

namespace UniGate.UserService.Services;

public class UserService(
    UserManager<User> userManager,
    ITokenService tokenService,
    ITokenStore tokenStore,
    IUserRepository userRepository)
    : IUserService
{
    public async Task<Result> UpdatePassword(Guid userId, UpdatePasswordDto updatePasswordDto)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null) return new Result { Code = HttpCode.NotFound, Message = "User not found" };

        var result =
            await userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword,
                updatePasswordDto.NewPassword);

        return !result.Succeeded
            ? new Result { Code = HttpCode.BadRequest, Message = result.Errors.First().Description }
            : new Result();
    }

    public async Task<Result<ProfileDto>> GetProfileDto(Guid userId)
    {
        var foundUser = await userRepository.RetrieveUser(userId);

        if (foundUser == null) return new Result<ProfileDto> { Message = "User not found", Code = HttpCode.NotFound };

        var userRoles = (await userManager.GetRolesAsync(foundUser)).ToList();

        return new Result<ProfileDto> { Data = foundUser.ToDto(userRoles) };
    }

    public async Task<Result<TokenDto>> Login(LoginDto loginDto)
    {
        var foundUser = await userManager.FindByEmailAsync(loginDto.Email);

        if (foundUser == null)
            return new Result<TokenDto> { Message = "Invalid credentials", Code = HttpCode.BadRequest };

        if (!await userManager.CheckPasswordAsync(foundUser, loginDto.Password))
            return new Result<TokenDto> { Message = "Invalid credentials", Code = HttpCode.BadRequest };

        var roles = (await userManager.GetRolesAsync(foundUser)).ToList();

        return new Result<TokenDto> { Data = await tokenService.GenerateTokens(foundUser.Id, roles) };
    }

    public async Task<Result<TokenDto>> RefreshToken(string refreshToken)
    {
        var userId = await tokenStore.GetUserIdByRefreshToken(refreshToken);

        if (userId == null)
            return new Result<TokenDto> { Code = HttpCode.BadRequest, Message = "Invalid refresh token" };

        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return new Result<TokenDto> { Message = "Invalid refresh token", Code = HttpCode.NotFound };

        var userRoles = await userManager.GetRolesAsync(user);

        return new Result<TokenDto>
        {
            Data = new TokenDto
            {
                AccessToken = tokenService.GenerateAccessToken(user.Id, userRoles.ToList()),
                RefreshToken = refreshToken
            }
        };
    }

    public async Task<Result<User>> CreateUser(RegisterUserDto registerUserDto)
    {
        var result = await userManager.CreateAsync(registerUserDto.ToUser(), registerUserDto.Password);

        if (!result.Succeeded)
            return new Result<User> { Code = HttpCode.BadRequest, Message = result.Errors.First().Description };

        var createdUser = await userManager.FindByEmailAsync(registerUserDto.Email);

        if (createdUser == null)
            return new Result<User>
                { Code = HttpCode.BadRequest, Message = "Something went wrong while creating user" };

        return new Result<User> { Data = createdUser };
    }

    public async Task<Result> UpdateProfile(Guid userId, UpdateProfileDto updateProfileDto)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

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

    public async Task<Result> DeleteProfile(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null) return new Result { Code = HttpCode.NotFound, Message = "User not found" };

        var result = await userManager.DeleteAsync(user);

        return result.Succeeded
            ? new Result()
            : new Result { Code = HttpCode.BadRequest, Message = result.Errors.First().Description };
    }
}