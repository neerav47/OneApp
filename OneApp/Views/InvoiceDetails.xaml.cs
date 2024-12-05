using OneApp.ViewModels;

namespace OneApp.Views;

public partial class InvoiceDetails : ContentPage
{
	public InvoiceDetails(InvoiceDetailsViewModel invoiceDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = invoiceDetailsViewModel;
	}
}
