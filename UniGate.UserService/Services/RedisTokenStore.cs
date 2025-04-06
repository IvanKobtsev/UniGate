using StackExchange.Redis;
using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Services;

public class RedisTokenStore(IConnectionMultiplexer redis) : ITokenStore
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task StoreRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry)
    {
        await _db.HashSetAsync("refreshToUser", refreshToken, userId);
        await _db.HashSetAsync("userToRefresh", userId, refreshToken);
    }

    public async Task<string?> GetUserIdByRefreshToken(string refreshToken)
    {
        return await _db.HashGetAsync("refreshToUser", refreshToken);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string userId)
    {
        var refreshToken = await _db.HashGetAsync("userToRefresh", userId);

        if (string.IsNullOrEmpty(refreshToken))
            return new Result
                { Code = HttpCode.BadRequest, Message = "Either token is invalid or user is already logged out" };

        await _db.HashDeleteAsync("refreshToUser", refreshToken);
        await _db.HashDeleteAsync("userToRefresh", userId);
        return new Result();
    }
}