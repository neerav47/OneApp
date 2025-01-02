using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.ViewModels;

[QueryProperty("InvoiceId", "InvoiceId")]
public partial class EditInvoiceViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<EditInvoiceViewModel> _logger;
    public EditInvoiceViewModel(
        ITransactionService transactionService,
        ILogger<EditInvoiceViewModel> logger)
    {
        this._transactionService = transactionService;
        this._logger = logger;
    }

    [ObservableProperty]
    string invoiceId;

    [ObservableProperty]
    Invoice _invoice;

    [ObservableProperty]
    bool _isLoading;

    [RelayCommand]
    public async Task Init()
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} started.");
        IsLoading = true;
        Invoice = await _transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        IsLoading = false;
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} completed.");
    }
}
