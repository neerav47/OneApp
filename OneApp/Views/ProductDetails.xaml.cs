using OneApp.ViewModels;

namespace OneApp.Views;

public partial class ProductDetails : ContentPage
{
	public ProductDetails(ProductDetailsViewModel _productDetailsVM)
	{
		InitializeComponent();
		BindingContext = _productDetailsVM;
	}
}
