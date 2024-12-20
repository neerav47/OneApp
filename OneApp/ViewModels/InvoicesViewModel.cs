using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;
using OneApp.Views;

namespace OneApp.ViewModels;

public partial class InvoicesViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<InvoicesViewModel> _logger;

    public InvoicesViewModel(
        ITransactionService transactionService,
        ILogger<InvoicesViewModel> logger)
	{
        this._transactionService = transactionService;
        this._logger = logger;
	}

    [ObservableProperty]
    public IList<string> _chips = new List<string>()
    {
        "All",
        Contracts.v1.Enums.Status.Created.ToString(),
        Contracts.v1.Enums.Status.Completed.ToString(),
        Contracts.v1.Enums.Status.Deleted.ToString(),
    };

    [ObservableProperty]
    IEnumerable<Invoice> _invoices;

    [ObservableProperty]
    bool _isLoading;

	[RelayCommand]
	public void AddInvoice()
	{
        Application.Current.MainPage.DisplayAlert("New invoice", "New invoice clicked", "Ok", "Cancel");
    }

    [RelayCommand]
    public async Task InvoiceSelected(Invoice invoice)
    {
        _logger.LogInformation($"{nameof(InvoiceSelected)} started.");
        await Shell.Current.GoToAsync($"{nameof(InvoiceDetails)}?InvoiceId={invoice.Id.ToString()}", true);
    }

    [RelayCommand]
    public async Task CreateInvoiceSelected()
    {
        _logger.LogInformation($"{nameof(InvoicesViewModel)}-{nameof(CreateInvoiceSelected)} started.");
        await Shell.Current.GoToAsync($"{nameof(CreateInvoice)}");
    }

    [RelayCommand]
    public async Task Load()
    {
        _logger.LogInformation($"{nameof(InvoicesViewModel)}-{nameof(Load)} started.");

        IsLoading = true;

        Invoices = await _transactionService.GetInvoices();

        IsLoading = false;

        _logger.LogInformation($"{nameof(InvoicesViewModel)}-{nameof(Load)} completed.");
    }
}

