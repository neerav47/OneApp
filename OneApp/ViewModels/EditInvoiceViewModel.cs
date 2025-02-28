using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;
using static OneApp.Constants.Enums;

namespace OneApp.ViewModels;

[QueryProperty("InvoiceId", "InvoiceId")]
public partial class EditInvoiceViewModel : ObservableObject
{
    private readonly ITransactionService _transactionService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ILogger<EditInvoiceViewModel> _logger;

    public EditInvoiceViewModel(
        ITransactionService transactionService,
        ICustomerService customerService,
        IProductService productService,
        ILogger<EditInvoiceViewModel> logger)
    {
        this._transactionService = transactionService;
        this._customerService = customerService;
        this._productService = productService;
        this._logger = logger;
    }

    [ObservableProperty]
    string invoiceId;

    [ObservableProperty]
    IEnumerable<Customer> _customers = [];

    [ObservableProperty]
    IEnumerable<Product> _products = [];

    [ObservableProperty]
    Invoice _invoice;

    [ObservableProperty]
    InvoiceItem _invoiceItem;

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

    [ObservableProperty]
    InvoiceItemMode _invoiceItemMode;

    [RelayCommand]
    public async Task Init()
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} started.");
        IsLoading = true;
        var invoicesTask = _transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        var customersTask = _customerService.GetCustomers();
        var productsTask = _productService.GetProducts();
        await Task.WhenAll(invoicesTask, customersTask, productsTask);
        Invoice = invoicesTask.Result;
        Customers = customersTask.Result;
        Products = productsTask.Result;
        BindProperties();
        IsLoading = false;
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} completed.");
    }

    [RelayCommand]
    public async Task DeleteInvoiceItem(InvoiceItem item)
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} started.");
        var response = await _transactionService.DeleteInvoiceItem(item.ReceiptId, item.Id);
        if (!response)
        {
            _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} completed with errors.");
            return;
        }
        await RefreshInvoice();
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} completed.");
    }

    private void BindProperties()
    {
        InvoiceDate = Invoice.CreatedDate;
        SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Invoice.CustomerId) ?? null;
        Total = Invoice.InvoiceItems?.Sum(i => i.Total) ?? 0;
    }

    public async Task LoadProducts()
    {
        if (Products.Any())
            return;
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(LoadProducts)} started.");
        Products = await _productService.GetProducts();
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(LoadProducts)} completed.");
    }

    public async Task AddOrEditItem()
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} started.");
        if (InvoiceItemMode == InvoiceItemMode.New)
        {
            var request = new AddInvoiceItemRequest
            {
                ReceiptId = Invoice.Id,
                ProductId = InvoiceItem.Product.Id,
                Quantity = InvoiceItem.Quantity,
                UnitPrice = InvoiceItem.UnitPrice
            };
            var response = await _transactionService.AddInvoiceItem(InvoiceId, request);
            if (response == default)
            {
                _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed with errors.");
                return;
            }
            await RefreshInvoice();
        }
        else
        {
            var request = new EditInvoiceItemRequest
            {
                Quantity = InvoiceItem.Quantity,
                UnitPrice = InvoiceItem.UnitPrice
            };
            var response = await _transactionService.EditInvoiceItem(InvoiceId, InvoiceItem.Id.ToString(), request);
            if (!response)
            {
                _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed with errors.");
                return;
            }
            await RefreshInvoice();
        }
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed.");
    }

    private async Task RefreshInvoice()
    {
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(RefreshInvoice)} started.");
        IsLoading = true;
        var invoice = await _transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        Invoice = invoice;
        BindProperties();
        IsLoading = false;
        _logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(RefreshInvoice)} completed.");
    }
}
