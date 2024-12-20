using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.ViewModels;

public partial class CreateInvoiceViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly ILogger<CreateInvoiceViewModel> _logger;
    public CreateInvoiceViewModel(
        IProductService productService,
        ICustomerService customerService,
        ILogger<CreateInvoiceViewModel> logger)
	{
        this._productService = productService;
        this._customerService = customerService;
        this._logger = logger;
	}

    [ObservableProperty]
    IEnumerable<Product> _products = Enumerable.Empty<Product>();

    [ObservableProperty]
    IEnumerable<Customer> _customers = Enumerable.Empty<Customer>();

    [ObservableProperty]
    Product? _selectedProduct = null;

    [ObservableProperty]
    Customer? _selectedCustomer = null;

    public async Task LoadProducts()
    {
        if(Products.Count() > 0) 
            return;
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadProducts)} started.");
        Products = await _productService.GetProducts();
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadProducts)} completed.");
    }

    public async Task LoadCustomers()
    {
        if(Customers.Count() > 0)
            return;
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadCustomers)} started.");
        Customers = await _customerService.GetCustomers();
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadCustomers)} completed.");
    }
}

