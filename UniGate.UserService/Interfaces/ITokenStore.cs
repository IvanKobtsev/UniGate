namespace UniGate.UserService.Interfaces;

public interface ITokenStore
{
    Task StoreRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry);
    Task<string?> GetUserIdByRefreshToken(string refreshToken);
    Task RevokeRefreshTokenAsync(string userId);
}