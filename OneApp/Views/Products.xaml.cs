using OneApp.ViewModels;

namespace OneApp.Views;

public partial class Products : ContentPage
{
	public Products(ProductViewModel _productViewModel)
	{
		InitializeComponent();
		BindingContext = _productViewModel;
	}
}
