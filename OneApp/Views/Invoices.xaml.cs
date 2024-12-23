using DevExpress.Maui.Controls;
using Microsoft.Extensions.Logging;
using OneApp.ViewModels;

namespace OneApp.Views;

public partial class Invoices : ContentPage
{
	private readonly InvoicesViewModel _invoicesVM;
	private readonly ILogger<InvoicesViewModel> _logger;
	public Invoices(
		InvoicesViewModel invoicesViewModel,
		ILogger<InvoicesViewModel> logger)
	{
		InitializeComponent();
		this._invoicesVM = invoicesViewModel;
		this._logger = logger;
		BindingContext = this._invoicesVM;
	}

	void OpenFilterPopup(Object sender, EventArgs e)
	{
		actionsPopup.PlacementTarget = (View)FilterFrame;
		actionsPopup.IsOpen = !actionsPopup.IsOpen;
	}
}