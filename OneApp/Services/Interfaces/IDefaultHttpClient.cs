using System;
namespace OneApp.Services;

public interface IDefaultHttpClient
{
    HttpRequestMessage CreateHttpRequestMessage(Uri requestUrl, HttpMethod httpMethod, string? token = null);

    Task<HttpResponseMessage> InvokeRequest(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> InvokeRequest(Uri requestUrl, HttpMethod httpMethod, string? requestBody, string token);

    string GetBaseAddress();
}

