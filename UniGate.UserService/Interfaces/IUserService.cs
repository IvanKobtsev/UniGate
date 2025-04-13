using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Models;

namespace UniGate.UserService.Interfaces;

public interface IUserService
{
    public Task<Result<User>> CreateUser(RegisterUserDto registerApplicantDto);
    public Task<Result<ProfileDto>> GetProfileDto(Guid userId);
    public Task<Result<TokenDto>> Login(LoginDto loginDto);
    public Task<Result<TokenDto>> RefreshToken(string refreshToken);
    public Task<Result> UpdateProfile(Guid userId, UpdateProfileDto updateProfileDto);
    public Task<Result> UpdatePassword(Guid userId, UpdatePasswordDto updatePasswordDto);
    public Task<Result> DeleteProfile(Guid userId);
}