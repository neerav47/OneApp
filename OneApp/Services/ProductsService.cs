using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.Services;

public class ProductsService : IProductService
{
	private readonly IDefaultHttpClient _httpClient;
    private readonly ILogger<ProductsService> _logger;
    private readonly IAuthenticationService _authenticationService;

    public ProductsService(IDefaultHttpClient httpClient, ILogger<ProductsService> logger,
        IAuthenticationService authenticationService)
	{
		this._httpClient = httpClient;
        this._logger = logger;
        this._authenticationService = authenticationService;
	}

	public async Task<IEnumerable<Product>> GetProducts()
	{
        _logger.LogInformation($"{nameof(GetProducts)} started");
        var userContext = await _authenticationService.GetUserContext();
        var request = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/v1/getProducts"),
            HttpMethod.Get,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(request);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }
        _logger.LogInformation($"{nameof(GetProducts)} completed.");

        return null;
    }
}

