using Microsoft.Extensions.Configuration;

namespace UniGate.Common.HMAC;

public class HmacHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var signatureData = $"{timestamp}";
        var signature = HmacSigner.ComputeSignature(signatureData,
            config["Hmac:Secret"] ??
            throw new InvalidOperationException("Missing secret key for HMAC in configuration"));

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("X-Timestamp", timestamp);
        request.Headers.Add("X-Signature", signature);

        return await httpClient.SendAsync(request);
    }
}