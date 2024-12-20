using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.Services;

public class CustomerService : ICustomerService
{
    private readonly IDefaultHttpClient _httpClient;
    private readonly ILogger<CustomerService> _logger;
    private readonly IAuthenticationService _authenticationService;

    public CustomerService(
        IDefaultHttpClient httpClient,
        ILogger<CustomerService> logger,
        IAuthenticationService authenticationService)
	{
		this._httpClient = httpClient;
        this._logger = logger;
        this._authenticationService = authenticationService;
	}
    public async Task<IEnumerable<Customer>> GetCustomers()
    {
        _logger.LogInformation($"{nameof(GetCustomers)} started");
        var userContext = _authenticationService.GetUserContext();
        var request = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/v1/customers"),
            HttpMethod.Get,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(request);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Customer>>(result);
        }
        _logger.LogInformation($"{nameof(GetCustomers)} completed.");

        return Enumerable.Empty<Customer>();
    }
}
