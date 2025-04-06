using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Enums;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Services;

public class TokenService(IConfiguration config, ITokenStore tokenStore) : ITokenService
{
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<TokenDto> GenerateTokens(string userId, Role userRole)
    {
        var tokenDto = new TokenDto
        {
            AccessToken = GenerateAccessToken(userId, userRole),
            RefreshToken = GenerateRefreshToken()
        };

        await tokenStore.StoreRefreshTokenAsync(userId, tokenDto.RefreshToken,
            TimeSpan.FromDays(int.Parse(config["JwtSettings:RefreshTokenExpiryDays"] ??
                                        throw new InvalidOperationException(
                                            "Jwt:Refresh token expiration is missing in configuration."))));

        return tokenDto;
    }

    public string GenerateAccessToken(string userId, Role userRole)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Secret"] ??
                                                                  throw new InvalidOperationException(
                                                                      "Jwt:Secret is missing in configuration.")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new("role", userRole.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            config["JwtSettings:Issuer"] ??
            throw new InvalidOperationException(
                "Jwt:Secret is missing in configuration."),
            config["JwtSettings:Audience"] ??
            throw new InvalidOperationException(
                "Jwt:Secret is missing in configuration."),
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["JwtSettings:AccessTokenExpiryMinutes"] ??
                                                              throw new InvalidOperationException(
                                                                  "Jwt:Expiry time for access token is missing in configuration."))),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}