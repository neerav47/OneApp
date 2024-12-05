using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.ViewModels;

[QueryProperty(nameof(ProductId), "productId")]
public partial class ProductDetailsViewModel: ObservableObject
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductDetailsViewModel> _logger;
    public ProductDetailsViewModel(
		IProductService productService,
        ILogger<ProductDetailsViewModel> logger)
	{
		this._productService = productService;
		this._logger = logger;
	}

	[ObservableProperty]
	string _productId;

    [ObservableProperty]
    bool _isLoading;

    [ObservableProperty]
    Product _product;

    [ObservableProperty]
    Invoice _selectedInvoice;

	public async Task Load()
	{
        _logger.LogInformation($"{nameof(ProductDetailsViewModel)}-{nameof(Load)} started.");

        IsLoading = true;

        Product = await _productService.GetProductById(ProductId);

        IsLoading = false;
    }
}

