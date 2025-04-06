using UniGate.Common.Utilities;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;

namespace UniGate.UserService.Interfaces;

public interface IUsersService
{
    public Task<Result<TokenDto>> Register(RegisterDto registerDto);
    public Task<Result<ProfileDto>> GetProfileDto(string userId);
    public Task<Result<TokenDto>> Login(LoginDto loginDto);
    public Task<Result<TokenDto>> RefreshToken(string refreshToken);
    public Task<Result> UpdateProfile(string userId, UpdateProfileDto updateProfileDto);
    public Task<Result> UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto);
}