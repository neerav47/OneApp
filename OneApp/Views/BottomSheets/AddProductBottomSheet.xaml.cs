using OneApp.Services.Interfaces;
using OneApp.ViewModels;
using The49.Maui.BottomSheet;

namespace OneApp.Views.BottomSheets;

public partial class AddProductBottomSheet : BottomSheet
{
	public AddProductBottomSheet(ProductsViewModel _productVM)
	{
		InitializeComponent();
		BindingContext = _productVM;
		ProductNameEntry.Focus();
	}

	public void Cancel_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		ProductNameEntry.HideSoftInputAsync(CancellationToken.None);
		DismissAsync();
    }
}
