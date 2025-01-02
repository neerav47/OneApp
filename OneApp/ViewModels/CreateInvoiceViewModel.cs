using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.ViewModels;

public partial class CreateInvoiceViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly ITransactionService _transactionService;
    private readonly ILogger<CreateInvoiceViewModel> _logger;
    public CreateInvoiceViewModel(
        IProductService productService,
        ICustomerService customerService,
        ITransactionService transactionService,
        ILogger<CreateInvoiceViewModel> logger)
    {
        this._productService = productService;
        this._customerService = customerService;
        this._transactionService = transactionService;
        this._logger = logger;
    }

    [ObservableProperty]
    IEnumerable<Product> _products = [];

    [ObservableProperty]
    IEnumerable<Customer> _customers = [];

    [ObservableProperty]
    ObservableCollection<NewInvoiceItem> _newInvoiceItems = [];

    [ObservableProperty]
    Product _selectedProduct = null;

    [ObservableProperty]
    Customer _selectedCustomer = null;

    [ObservableProperty]
    string _customersAutoCompleteEditErrorText = string.Empty;

    [ObservableProperty]
    bool _hasCustomersAutoCompleteEditErrors = false;

    [ObservableProperty]
    string _itemAutoCompleteEditErrorText = string.Empty;

    [ObservableProperty]
    bool _hasItemAutoCompleteEditErrors = false;

    [ObservableProperty]
    int _unitPrice = 0;

    [ObservableProperty]
    string _unitPriceTextEditErrorText = string.Empty;

    [ObservableProperty]
    bool _hasUnitPriceTextEditErrors = false;

    [ObservableProperty]
    int _quantity = 0;

    [ObservableProperty]
    string _quantityTextEditErrorText = string.Empty;

    [ObservableProperty]
    bool _hasQuantityTextEditErrors = false;

    [ObservableProperty]
    int _total = 0;

    [ObservableProperty]
    int _subTotal = 0;

    [ObservableProperty]
    DateTime _invoiceDate = DateTime.Now.Date;
    public async Task LoadProducts()
    {
        if (Products.Count() > 0)
            return;
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(LoadProducts)} started.");
        Products = await _productService.GetProducts();
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(LoadProducts)} completed.");
    }

    public async Task LoadCustomers()
    {
        if (Customers.Count() > 0)
            return;
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(LoadCustomers)} started.");
        Customers = await _customerService.GetCustomers();
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(LoadCustomers)} completed.");
    }

    [RelayCommand]
    async Task SaveInvoice()
    {
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(SaveInvoice)} started.");
        if (SelectedCustomer is null)
        {
            CustomersAutoCompleteEditErrorText = "Please select a customer";
            HasCustomersAutoCompleteEditErrors = true;
            return;
        }

        var request = new CreateInvoiceRequest
        {
            CustomerId = SelectedCustomer.Id,
            NewInvoiceItems = NewInvoiceItems.ToList()
        };
        var response = await _transactionService.CreateInvoice(request);
        if (response)
        {
            await Shell.Current.GoToAsync("..", true);
        }
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(SaveInvoice)} completed.");
    }

    public bool AddItem()
    {
        bool hasErrors = false;
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(AddItem)} started.");
        // Validate Selected Item
        if (SelectedProduct is null)
        {
            ItemAutoCompleteEditErrorText = "Please select an item";
            HasItemAutoCompleteEditErrors = true;
            hasErrors = true;
        }
        // Validate UnitPrice
        if (UnitPrice == 0)
        {
            UnitPriceTextEditErrorText = "Unit price must be greater than 0";
            HasUnitPriceTextEditErrors = true;
            hasErrors = true;
        }
        // Validate Quantity
        if (Quantity == 0)
        {
            QuantityTextEditErrorText = "Quantity must be greater that 0";
            HasQuantityTextEditErrors = true;
            hasErrors = true;
        }
        if (hasErrors)
        {
            return hasErrors;
        }

        var newItem = new NewInvoiceItem
        {
            TempId = Guid.NewGuid(),
            ProductId = SelectedProduct.Id,
            UnitPrice = UnitPrice,
            Quantity = Quantity,
            ProductName = SelectedProduct.Name
        };
        NewInvoiceItems.Add(newItem);
        _logger.LogInformation($"{nameof(CreateInvoiceViewModel)}-{nameof(AddItem)} completed.");
        return hasErrors;
    }

    public void ClearAddItemsBottomSheetFields()
    {
        // Items Edit field
        SelectedProduct = null;
        ItemAutoCompleteEditErrorText = string.Empty;
        HasItemAutoCompleteEditErrors = false;

        // Unit price field
        UnitPrice = 0;
        UnitPriceTextEditErrorText = string.Empty;
        HasUnitPriceTextEditErrors = false;

        // Quantity
        Quantity = 0;
        QuantityTextEditErrorText = string.Empty;
        HasQuantityTextEditErrors = false;

        // Subtotal
        SubTotal = 0;
    }

    public void CalculateTotal()
    {
        Total = NewInvoiceItems?.Sum(i => i.Total) ?? 0;
    }
}

