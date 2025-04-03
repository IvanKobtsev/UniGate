using Microsoft.AspNetCore.Identity;
using UniGate.Common.Exceptions;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
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
    public async Task<TokenDto> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Patronymic = registerDto.Patronymic ?? string.Empty,
            PhoneNumber = registerDto.PhoneNumber ?? string.Empty
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) throw new ValidationException(result.Errors.First().Description);

        var createdUser = await userManager.FindByEmailAsync(registerDto.Email);

        if (createdUser == null) throw new InternalServerException("Something went wrong while creating user");

        return await tokenService.GenerateTokens(createdUser.Id);
    }

    public async Task<ProfileDto> GetProfileDto(string userId)
    {
        return (await usersRepository.GetUser(userId)).ToDto();
    }

    public async Task<TokenDto> Login(LoginDto loginDto)
    {
        var foundUser = await userManager.FindByEmailAsync(loginDto.Email);

        if (foundUser == null) throw new NotFoundException("Invalid credentials");

        if (!await userManager.CheckPasswordAsync(foundUser, loginDto.Password))
            throw new ValidationException("Invalid credentials");

        return await tokenService.GenerateTokens(foundUser.Id);
    }

    public async Task<TokenDto> RefreshToken(string refreshToken)
    {
        var userId = await tokenStore.GetUserIdByRefreshToken(refreshToken);

        if (userId == null) throw new NotFoundException("Invalid refresh token");

        var user = await userManager.FindByIdAsync(userId);

        if (user == null) throw new NotFoundException("Invalid refresh token");

        return new TokenDto
        {
            AccessToken = tokenService.GenerateAccessToken(user.Id),
            RefreshToken = refreshToken
        };
    }

    public async Task UpdateProfile(string userId, UpdateProfileDto updateProfileDto)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) throw new NotFoundException("User not found");

        user.UserName = updateProfileDto.Email;
        user.Email = updateProfileDto.Email;
        user.FirstName = updateProfileDto.FirstName;
        user.LastName = updateProfileDto.LastName;
        user.Patronymic = updateProfileDto.Patronymic;
        user.PhoneNumber = updateProfileDto.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded) throw new ValidationException(result.Errors.First().Description);
    }

    public async Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) throw new NotFoundException("User not found");

        var result =
            await userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword,
                updatePasswordDto.NewPassword);

        if (!result.Succeeded) throw new ValidationException(result.Errors.First().Description);
    }
}