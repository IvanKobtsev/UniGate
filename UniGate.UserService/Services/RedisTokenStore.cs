using StackExchange.Redis;
using UniGate.UserService.Interfaces;
using ValidationException = UniGate.Common.Exceptions.ValidationException;

namespace UniGate.UserService.Services;

public class RedisTokenStore(IConnectionMultiplexer redis) : ITokenStore
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task StoreRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry)
    {
        await _db.HashSetAsync("refreshToUser", refreshToken, userId);
        await _db.HashSetAsync("userToRefresh", userId, refreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string userId)
    {
        var refreshToken = await _db.HashGetAsync("userToRefresh", userId);

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _db.HashDeleteAsync("refreshToUser", refreshToken);
            await _db.HashDeleteAsync("userToRefresh", userId);
        }
        else
        {
            throw new ValidationException("Either token is invalid or user is already logged out");
        }
    }

    public async Task<string?> GetUserIdByRefreshToken(string refreshToken)
    {
        return await _db.HashGetAsync("refreshToUser", refreshToken);
    }
}