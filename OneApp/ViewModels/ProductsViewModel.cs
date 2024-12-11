using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Enums;
using OneApp.Contracts.v1.Request;
using OneApp.Contracts.v1.Response;
using OneApp.Services.Interfaces;
using OneApp.Views;
using OneApp.Views.BottomSheets;
using The49.Maui.BottomSheet;

namespace OneApp.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
	private readonly IProductService _productService;
	private readonly ILogger<ProductsViewModel> _logger;
	private InventoryBottomSheet _inventoryBottomSheet;
	private AddProductBottomSheet _addProductBottomSheet;

    public ProductsViewModel(
		IProductService productService,
		ILogger<ProductsViewModel> logger)
	{
		this._productService = productService;
		this._logger = logger;
		ChangeValue = 0;
		this.NewValue = SelectedProduct?.Inventory?.Quantity ?? 0;
        Products = new ObservableCollection<Product>();
		ProductTypes = new ObservableCollection<ProductType>();
		SelectedFilters = new List<ProductType>();
    }

    [ObservableProperty]
	bool _isLoading;

	[ObservableProperty]
	Product _selectedProduct;

	[ObservableProperty]
	IEnumerable<Product> _products;

	[ObservableProperty]
	bool _inProgress;

	[ObservableProperty]
	int? _changeValue = 0;

	[ObservableProperty]
	int _updateInventoryType = (int)InventoryUpdateType.Add;

	[ObservableProperty]
	int _newValue;

	[ObservableProperty]
	string _productName = string.Empty;

	[ObservableProperty]
	string _productDescription = string.Empty;

	[ObservableProperty]
	int _selectProductTypeIndex = -1;

	[ObservableProperty]
	IEnumerable<ProductType> productTypes;

	[ObservableProperty]
    IList<ProductType> _selectedFilters;

	[ObservableProperty]
	string _searchText = string.Empty;

	partial void OnChangeValueChanging(int? value)
	{	
		var updateType = (InventoryUpdateType)UpdateInventoryType;
        var currentQuantity = SelectedProduct?.Inventory?.Quantity ?? 0;
        if (updateType == InventoryUpdateType.Add)
        {
            NewValue = currentQuantity + (value ?? 0);
        }
        else
        {
            NewValue = currentQuantity - (value ?? 0);
        }
    }

	public async Task Load()
	{
		_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(Load)} started.");
		Products = await _productService.GetProducts();
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(Load)} completed.");
    }

	public async Task LoadProductTypes()
	{
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadProductTypes)} started.");
        ProductTypes = await _productService.GetProductTypes();
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(LoadProductTypes)} completed.");
    }

	[RelayCommand]
	private async Task ProductSelected(Product product)
	{
		_logger.LogInformation($"{nameof(ProductSelected)} started.");

		await Shell.Current.GoToAsync($"{nameof(ProductDetails)}?productId={product.Id.ToString()}", true);
    }

	[RelayCommand]
	private void EditInventory(Product product)
	{
		_logger.LogInformation($"{nameof(EditInventory)} invoked.");
		SelectedProduct = product;
		_inventoryBottomSheet = new InventoryBottomSheet(this);
		_inventoryBottomSheet.IsCancelable = false;
		_inventoryBottomSheet.HasBackdrop = true;
		_inventoryBottomSheet.ShowAsync();
	}

    private void OnAddProductBottomSheetDismiss(object sender, DismissOrigin e)
    {
		ProductName = string.Empty;
		ProductDescription = string.Empty;
		SelectProductTypeIndex = -1;
    }

    
	[RelayCommand]
	async Task UpdateInventory()
	{
		_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(UpdateInventory)} started.");
		InProgress = true;
		var request = new UpdateInventoryRequest
		{
			ProductId = SelectedProduct.Id,
			InventoryUpdateType = (InventoryUpdateType)UpdateInventoryType,
			Value = ChangeValue.Value
		};

		var response = await _productService.UpdateInventory(request);
		if (response)
		{
			InProgress = false;
			ChangeValue = 0;
			NewValue = 0;
			await _inventoryBottomSheet.DismissAsync();
			await Load();
		}
		else
		{
            InProgress = !response;
        }
    }

	[RelayCommand]
	void OpenAddProductBottomSheet()
	{
        _addProductBottomSheet = new AddProductBottomSheet(this);
        _addProductBottomSheet.IsCancelable = false;
        _addProductBottomSheet.HasBackdrop = true;
        _addProductBottomSheet.Dismissed += OnAddProductBottomSheetDismiss;
        _addProductBottomSheet.ShowAsync();
    }

	[RelayCommand]
	async Task AddProduct()
	{
        _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(AddProduct)} started.");
        InProgress = true;

        var request = new CreateProductRequest
        {
            Name = ProductName.Trim(),
			Description = ProductDescription.Trim(),
			ProductTypeId = ProductTypes.ElementAt(SelectProductTypeIndex).Id.ToString()
        };

        var response = await _productService.CreateProduct(request);
        if (response)
        {
            InProgress = false;
            await _addProductBottomSheet.DismissAsync();
            await Load();
        }
        else
        {
            InProgress = !response;
        }
    }

	[RelayCommand]
	public Task OnFilterChange()
	{
		if(SelectedFilters?.Count() > 0)
		{
            _logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(OnFilterChange)} started.");
			var productTypeIds = SelectedFilters.Select(s => s.Id);

			Products = Products.Where(p => productTypeIds.Contains(p.ProductType.Id)).AsEnumerable();

			_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(OnFilterChange)} completed.");
        }
		return Task.CompletedTask;
    }

	[RelayCommand]
	void OnSearch()
	{
		_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(OnSearch)} started.");
		var text = SearchText;

		_logger.LogInformation($"{nameof(ProductsViewModel)}-{nameof(OnSearch)} completed.");
	}
}

