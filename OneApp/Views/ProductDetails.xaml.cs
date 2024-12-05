using OneApp.ViewModels;

namespace OneApp.Views;

public partial class ProductDetails : ContentPage
{
	private readonly ProductDetailsViewModel _productDetailsVM;

    public ProductDetails(ProductDetailsViewModel productDetailsVM)
	{
		InitializeComponent();
		this._productDetailsVM = productDetailsVM;
		BindingContext = _productDetailsVM;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = this._productDetailsVM.Load();
    }
}
