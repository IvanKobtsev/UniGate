using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Enums;

namespace UniGate.UserService.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(string userId, Role userRole);

    public string GenerateRefreshToken();
    public Task<TokenDto> GenerateTokens(string userId, Role userRole);
}