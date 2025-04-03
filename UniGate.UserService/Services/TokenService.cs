using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Interfaces;

namespace UniGate.UserService.Services;

public class TokenService(IConfiguration config, ITokenStore tokenStore) : ITokenService
{
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public string GenerateAccessToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Secret"] ??
                                                                  throw new InvalidOperationException(
                                                                      "Jwt:Secret is missing in configuration.")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            config["JwtSettings:Issuer"],
            config["JwtSettings:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["JwtSettings:AccessTokenExpiryMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<TokenDto> GenerateTokens(string userId)
    {
        var tokenDto = new TokenDto
        {
            AccessToken = GenerateAccessToken(userId),
            RefreshToken = GenerateRefreshToken()
        };

        await tokenStore.StoreRefreshTokenAsync(userId, tokenDto.RefreshToken,
            TimeSpan.FromDays(int.Parse(config["JwtSettings:RefreshTokenExpiryDays"] ??
                                        throw new InvalidOperationException(
                                            "Jwt:Refresh token expiration is missing in configuration."))));

        return tokenDto;
    }
}