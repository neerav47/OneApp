using DevExpress.Maui.Controls;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.ViewModels;
using static OneApp.Constants.Enums;

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

	void OpenAddOrEditInvoiceItemBottomSheet(object sender, EventArgs e)
	{
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddOrEditInvoiceItemBottomSheet)} started.");
		if (sender.GetType() == typeof(ImageButton))
		{
			this._editInvoiceVM.InvoiceItem = new InvoiceItem();
			this._editInvoiceVM.InvoiceItemMode = InvoiceItemMode.New;
		}
		else
		{
			var item = (InvoiceItem)((SwipeItemView)sender).CommandParameter;
			this._editInvoiceVM.InvoiceItem = new InvoiceItem
			{
				Id = item.Id,
				ProductId = item.ProductId,
				Quantity = item.Quantity,
				UnitPrice = item.UnitPrice,
				Product = item.Product
			};
			this._editInvoiceVM.InvoiceItemMode = InvoiceItemMode.Edit;
		}
		this._editInvoiceVM.LoadProducts().GetAwaiter().GetResult();
		addItemsBottomSheet.State = BottomSheetState.HalfExpanded;
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddOrEditInvoiceItemBottomSheet)} completed.");
	}

	void EditInvoiceItem_CancelButtonPressed(object sender, EventArgs e)
	{
		addItemsBottomSheet.State = BottomSheetState.Hidden;
		// Clear Invoice Item fields
		this._editInvoiceVM.InvoiceItem = null;
		this._editInvoiceVM.InvoiceItemMode = InvoiceItemMode.New;
	}

	void EditInvoiceItem_AddButtonPressed(object sender, EventArgs e)
	{
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(EditInvoiceItem_AddButtonPressed)} started.");
		this._editInvoiceVM.AddOrEditItem();
		addItemsBottomSheet.State = BottomSheetState.Hidden;
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(EditInvoiceItem_AddButtonPressed)} completed.");
	}
}