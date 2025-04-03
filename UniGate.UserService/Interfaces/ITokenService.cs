using UniGate.UserService.DTOs.Common;

namespace UniGate.UserService.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(string userId);

    public string GenerateRefreshToken();
    public Task<TokenDto> GenerateTokens(string userId);
}