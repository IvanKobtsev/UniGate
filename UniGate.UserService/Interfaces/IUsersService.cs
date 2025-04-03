using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;

namespace UniGate.UserService.Interfaces;

public interface IUsersService
{
    public Task<TokenDto> Register(RegisterDto registerDto);
    public Task<ProfileDto> GetProfileDto(string userId);
    public Task<TokenDto> Login(LoginDto loginDto);
    public Task<TokenDto> RefreshToken(string refreshToken);
    public Task UpdateProfile(string userId, UpdateProfileDto updateProfileDto);
    public Task UpdatePassword(string userId, UpdatePasswordDto updatePasswordDto);
}