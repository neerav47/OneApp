using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1.Request;
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
        var userContext = _authenticationService.GetUserContext();
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

    public async Task<bool> UpdateInventory(UpdateInventoryRequest request)
    {
        _logger.LogInformation($"{nameof(ProductsService)}-{nameof(UpdateInventory)} started");
        var userContext = _authenticationService.GetUserContext();

        var requestMessage = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/Inventory"),
            HttpMethod.Put,
            userContext.AccessToken);

        var requestBody = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Content = content;

        var response = await _httpClient.InvokeRequest(requestMessage);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }
        _logger.LogInformation($"{nameof(GetProducts)} completed.");

        return false;
    }

    public async Task<IEnumerable<ProductType>> GetProductTypes()
    {
        _logger.LogInformation($"{nameof(ProductsService)}-{nameof(GetProductTypes)} started");
        var userContext = _authenticationService.GetUserContext();

        var requestMessage = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/v1/getProductTypes"),
            HttpMethod.Get,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(requestMessage);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ProductType>>(result);
        }
        _logger.LogInformation($"{nameof(GetProductTypes)} completed.");

        return Enumerable.Empty<ProductType>();
    }

    public async Task<bool> CreateProduct(CreateProductRequest request)
    {
        _logger.LogInformation($"{nameof(ProductsService)}-{nameof(CreateProduct)} started");
        var userContext = _authenticationService.GetUserContext();

        var requestMessage = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/v1/product"),
            HttpMethod.Post,
            userContext.AccessToken);

        var requestBody = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Content = content;

        var response = await _httpClient.InvokeRequest(requestMessage);

        _logger.LogInformation($"{nameof(CreateProduct)} completed.");

        return response?.IsSuccessStatusCode == true;
    }
}

