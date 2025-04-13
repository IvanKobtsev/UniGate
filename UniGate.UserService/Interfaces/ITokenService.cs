using UniGate.UserService.DTOs.Common;

namespace UniGate.UserService.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(Guid userId, List<string> userRoles);

    public string GenerateRefreshToken();
    public Task<TokenDto> GenerateTokens(Guid userId, List<string> userRoles);
}