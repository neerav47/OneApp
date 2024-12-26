using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneApp.Contracts.v1.Enums;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.Services;

public class TransactionService : ITransactionService
{
    private readonly IDefaultHttpClient _httpClient;
    private readonly ILogger<ProductsService> _logger;
    private readonly IAuthenticationService _authenticationService;

    public TransactionService(IDefaultHttpClient httpClient, ILogger<ProductsService> logger,
        IAuthenticationService authenticationService)
    {
        this._httpClient = httpClient;
        this._logger = logger;
        this._authenticationService = authenticationService;
    }

    public async Task<IEnumerable<Invoice>> GetInvoices(Status? status, Guid? userId)
    {
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(GetInvoices)} started");
        var userContext = await _authenticationService.GetUserContext();
        var request = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/Transactional/v1/invoices"),
            HttpMethod.Get,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(request);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Invoice>>(result);
        }
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(GetInvoices)} completed.");

        return Enumerable.Empty<Invoice>();
    }

    public async Task<Invoice> GetInvoiceById(Guid id)
    {
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(GetInvoices)} started");
        var userContext = await _authenticationService.GetUserContext();
        var request = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/Transactional/v1/invoice/{id}"),
            HttpMethod.Get,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(request);

        if (response?.IsSuccessStatusCode == true)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Invoice>(result);
        }
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(GetInvoiceById)} completed.");

        return default;
    }

    public async Task<bool> CreateInvoice(CreateInvoiceRequest request)
    {
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(CreateInvoice)} started");
        var userContext = await _authenticationService.GetUserContext();

        var requestMessage = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/Transactional/v1/invoice"),
            HttpMethod.Post,
            userContext.AccessToken);

        var requestBody = JsonConvert.SerializeObject(request);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        requestMessage.Content = content;

        var response = await _httpClient.InvokeRequest(requestMessage);

        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(CreateInvoice)} completed.");

        return response?.IsSuccessStatusCode == true;
    }

    public async Task<bool> DeleteInvoiceById(string id)
    {
        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(DeleteInvoiceById)} started");
        var userContext = await _authenticationService.GetUserContext();
        var request = _httpClient.CreateHttpRequestMessage(
            new Uri($"{_httpClient.GetBaseAddress()}/api/Transactional/v1/invoice/{id}"),
            HttpMethod.Delete,
            userContext.AccessToken);

        var response = await _httpClient.InvokeRequest(request);

        _logger.LogInformation($"{nameof(TransactionService)}-{nameof(DeleteInvoiceById)} completed.");

        return response?.IsSuccessStatusCode == true;
    }
}

