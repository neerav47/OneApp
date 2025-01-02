using OneApp.ViewModels;

namespace OneApp.Views.Invoice;

public partial class EditInvoice : ContentPage
{
	public EditInvoice(EditInvoiceViewModel editInvoiceViewModel)
	{
		InitializeComponent();
		BindingContext = editInvoiceViewModel;
	}
}