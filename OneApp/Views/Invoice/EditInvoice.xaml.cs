using Microsoft.Extensions.Logging;
using OneApp.ViewModels;

namespace OneApp.Views.Invoice;

public partial class EditInvoice : ContentPage
{
	private readonly EditInvoiceViewModel _editInvoiceVM;
	private readonly ILogger<EditInvoice> _logger;
	public EditInvoice(EditInvoiceViewModel editInvoiceViewModel, ILogger<EditInvoice> logger)
	{
		InitializeComponent();
		this._editInvoiceVM = editInvoiceViewModel;
		this._logger = logger;
		BindingContext = this._editInvoiceVM;
	}

	private void customersAutoCompleteEdit_SelectionChanged(object sender, EventArgs e)
	{
		if (this._editInvoiceVM.SelectedCustomer is not null)
		{
			this._editInvoiceVM.CustomersAutoCompleteEditErrorText = string.Empty;
			this._editInvoiceVM.HasCustomersAutoCompleteEditErrors = false;
		}
	}

	void OpenAddItemsBottomSheet(System.Object sender, EventArgs e)
	{
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddItemsBottomSheet)} started.");
		// Task.Run(async () =>
		// {
		// 	//await Task.WhenAll(this._createInvoiceVM.LoadProducts(), this._createInvoiceVM.LoadCustomers());
		// 	_logger.LogInformation("Loading products, customers complete.");
		// });
		//addItemsBottomSheet.State = BottomSheetState.HalfExpanded;
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddItemsBottomSheet)} completed.");
	}
}