using UniGate.Common.Utilities;

namespace UniGate.UserService.Interfaces;

public interface ITokenStore
{
    Task StoreRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry);
    Task<string?> GetUserIdByRefreshToken(string refreshToken);
    Task<Result> RevokeRefreshTokenAsync(string userId);
}