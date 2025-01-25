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
    private readonly ICustomerService _customerService;
    private readonly ILogger<EditInvoiceViewModel> _logger;
    public EditInvoiceViewModel(
        ITransactionService transactionService,
        ICustomerService customerService,
        ILogger<EditInvoiceViewModel> logger)
    {
        this._transactionService = transactionService;
        this._customerService = customerService;
        this._logger = logger;
    }

    [ObservableProperty]
    string invoiceId;

    [ObservableProperty]
    IEnumerable<Customer> _customers = [];

    [ObservableProperty]
    Invoice _invoice;

    [ObservableProperty]
    bool _isLoading;

    [ObservableProperty]
    DateTime _invoiceDate = DateTime.Now.Date;

    [ObservableProperty]
    Customer _selectedCustomer = null;

    [ObservableProperty]
    string _customersAutoCompleteEditErrorText = string.Empty;

    [ObservableProperty]
    bool _hasCustomersAutoCompleteEditErrors = false;

    [ObservableProperty]
    decimal _total = 0;

    [RelayCommand]
    public async Task Init()
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} started.");
        IsLoading = true;
        var invoicesTask = _transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        var customersTask = _customerService.GetCustomers();
        await Task.WhenAll(invoicesTask, customersTask);
        Invoice = invoicesTask.Result;
        Customers = customersTask.Result;
        BindProperties();
        IsLoading = false;
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} completed.");
    }

    private void BindProperties()
    {
        InvoiceDate = Invoice.CreatedDate;
        SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Invoice.CustomerId) ?? null;
        Total = Invoice.InvoiceItems?.Sum(i => i.Total) ?? 0;
    }
}
