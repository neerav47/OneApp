using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;
using static OneApp.Constants.Enums;

namespace OneApp.ViewModels;

[QueryProperty("InvoiceId", "InvoiceId")]
public partial class EditInvoiceViewModel(
    ITransactionService transactionService,
    ICustomerService customerService,
    IProductService productService,
    ILogger<EditInvoiceViewModel> logger)
    : ObservableObject
{
    [ObservableProperty] private string invoiceId;

    [ObservableProperty] private IEnumerable<Customer> _customers = [];

    [ObservableProperty] private IEnumerable<Product> _products = [];

    [ObservableProperty] private Invoice _invoice;

    [ObservableProperty] private InvoiceItem _invoiceItem;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private DateTime _invoiceDate = DateTime.Now.Date;

    [ObservableProperty] private Customer _selectedCustomer = null;

    [ObservableProperty] private string _customersAutoCompleteEditErrorText = string.Empty;

    [ObservableProperty] private bool _hasCustomersAutoCompleteEditErrors = false;

    [ObservableProperty] private decimal _total = 0;

    [ObservableProperty] private InvoiceItemMode _invoiceItemMode;

    [RelayCommand]
    public async Task Init()
    {
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} started.");
        IsLoading = true;
        var invoicesTask = transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        var customersTask = customerService.GetCustomers();
        var productsTask = productService.GetProducts();
        await Task.WhenAll(invoicesTask, customersTask, productsTask);
        Invoice = invoicesTask.Result;
        Customers = customersTask.Result;
        Products = productsTask.Result;
        Products = productsTask.Result;
        BindProperties();
        IsLoading = false;
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(Init)} completed.");
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
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(LoadProducts)} started.");
        Products = await productService.GetProducts();
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(LoadProducts)} completed.");
    }

    public async Task AddOrEditItem()
    {
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} started.");
        if (InvoiceItemMode == InvoiceItemMode.New)
        {
            var request = new AddInvoiceItemRequest
            {
                ReceiptId = Invoice.Id,
                ProductId = InvoiceItem.Product.Id,
                Quantity = InvoiceItem.Quantity,
                UnitPrice = InvoiceItem.UnitPrice
            };
            var response = await transactionService.AddInvoiceItem(InvoiceId, request);
            if (response == default)
            {
                logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed with errors.");
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
            var response = await transactionService.EditInvoiceItem(InvoiceId, InvoiceItem.Id.ToString(), request);
            if (!response)
            {
                logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed with errors.");
                return;
            }
            await RefreshInvoice();
        }
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(AddOrEditItem)} completed.");
    }

    [RelayCommand]
    public async Task DeleteInvoiceItem(InvoiceItem item)
    {
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} started.");
        var response = await transactionService.DeleteInvoiceItem(item.ReceiptId, item.Id);
        if (!response)
        {
            logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} completed with errors.");
            return;
        }
        await RefreshInvoice();
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(DeleteInvoiceItem)} completed.");
    }

    private async Task RefreshInvoice()
    {
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(RefreshInvoice)} started.");
        IsLoading = true;
        var invoice = await transactionService.GetInvoiceById(Guid.Parse(InvoiceId));
        Invoice = invoice;
        BindProperties();
        IsLoading = false;
        logger.LogInformation($"{nameof(EditInvoiceViewModel)}-{nameof(RefreshInvoice)} completed.");
    }
}
