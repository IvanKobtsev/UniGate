using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace UniGate.Common.HMAC;

public class HmacAuthHandler(IConfiguration config)
{
    public async Task<bool> IsRequestValidAsync(HttpRequest request)
    {
        if (config["Hmac:AllowedClockSkewInMinutes"] == null)
            throw new InvalidOperationException("Missing AllowedClockSkewInMinutes in HMAC configuration");

        if (config["Hmac:Secret"] == null)
            throw new InvalidOperationException("Missing Secret in HMAC configuration");

        var timestampHeader = request.Headers["X-Timestamp"].FirstOrDefault();
        var signatureHeader = request.Headers["X-Signature"].FirstOrDefault();

        if (timestampHeader == null || signatureHeader == null)
            return false;

        if (!long.TryParse(timestampHeader, out var timestamp))
            return false;

        var requestTime = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        var now = DateTimeOffset.UtcNow;


        if (Math.Abs((now - requestTime).TotalSeconds) > double.Parse(config["Hmac:AllowedClockSkewInMinutes"]!) * 60)
            return false;

        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        var signatureData = $"{timestampHeader}";
        var expectedSignature = HmacSigner.ComputeSignature(signatureData, config["Hmac:Secret"]!);

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(signatureHeader),
            Encoding.UTF8.GetBytes(expectedSignature));
    }
}