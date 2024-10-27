using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;

namespace OneApp.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
	private readonly IProductService _productService;
	private readonly ILogger<ProductsViewModel> _logger;

	public ProductsViewModel(IProductService productService, ILogger<ProductsViewModel> logger)
	{
		this._productService = productService;
		this._logger = logger;
	}

	[ObservableProperty]
	bool _isLoading;

	[ObservableProperty]
	Product _selectedProduct;

	[ObservableProperty]
	ObservableCollection<Product> _products;

	public async Task<IEnumerable<Product>> Load()
	{
		_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(Load)} started.");
		IsLoading = true;
		Products = new ObservableCollection<Product>();
		await Task.Delay(1000);

		var response = await _productService.GetProducts();

		foreach (var p in response)
		{
			Products.Add(p);
		}

		IsLoading = false;

        return response;
	}

	[RelayCommand]
	private void ProductSelected(dynamic product)
	{
		_logger.LogInformation($"{nameof(OnProductsChanged)} started.");

        Application.Current.MainPage.DisplayAlert("Product", $"Name: {product.Name}", "Ok", "Cancel");
    }

	[RelayCommand]
	private void EditInventory(Product product)
	{
		_logger.LogInformation($"{nameof(EditInventory)} invoked.");

		Application.Current.MainPage.DisplayAlert("Inventory", $"Name: {product.Name}", "Ok", "Cancel");
	}
}

