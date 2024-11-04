using OneApp.ViewModels;
using The49.Maui.BottomSheet;
using CommunityToolkit.Maui.Core.Platform;

namespace OneApp.Views.BottomSheets;

public partial class InventoryBottomSheet : BottomSheet
{
	private readonly ProductsViewModel _productsVM;
	public InventoryBottomSheet(ProductsViewModel productsVM)
	{
		InitializeComponent();
		_productsVM = productsVM;
		BindingContext = _productsVM;
    }

    void Dismiss(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        this._productsVM.ChangeValue = 0;
        this._productsVM.NewValue = 0;
        this.newValueEntry.HideKeyboardAsync(CancellationToken.None);
        this.DismissAsync();
    }
}
