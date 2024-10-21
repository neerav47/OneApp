using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace OneApp.Services;

public class DefaultHttpClient : IDefaultHttpClient
{
    private readonly HttpClient _httpClient;

    public DefaultHttpClient(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public HttpRequestMessage CreateHttpRequestMessage(Uri requestUrl, HttpMethod httpMethod, string token = null)
    {
        var httpRequest = new HttpRequestMessage(httpMethod, requestUrl);

        if (token != null)
        {
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
        }

        var requestId = Guid.NewGuid().ToString();

        httpRequest.Headers.Add("x-client-request-id", requestId);

        return httpRequest;
    }

    public async Task<HttpResponseMessage> InvokeRequest(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
    {

        if(requestMessage == null || requestMessage.RequestUri == null)
        {
            throw new Exception("Invalid Http request");
        }

        var responseMessage = await this._httpClient.SendAsync(requestMessage, cancellationToken);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> InvokeRequest(Uri requestUrl, HttpMethod httpMethod, string requestBody, string token)
    {

        var httpRequestMessage = this.CreateHttpRequestMessage(requestUrl, httpMethod, token);

        if (!string.IsNullOrEmpty(requestBody))
        {
            httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        return await this.InvokeRequest(httpRequestMessage);
    }

    public string GetBaseAddress()
    {
        return DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7106" : "http://localhost:5274";
    }
}

