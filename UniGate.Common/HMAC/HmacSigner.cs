using System.Security.Cryptography;
using System.Text;

namespace UniGate.Common.HMAC;

public static class HmacSigner
{
    public static string ComputeSignature(string data, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}