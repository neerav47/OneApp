using DevExpress.Maui.Controls;
using Microsoft.Extensions.Logging;
using OneApp.Contracts.v1.Response;
using OneApp.ViewModels;
using static OneApp.Constants.Enums;

namespace OneApp.Views.Invoice;

public partial class EditInvoice : ContentPage
{
	private readonly EditInvoiceViewModel _editInvoiceVm;
	private readonly ILogger<EditInvoice> _logger;
	public EditInvoice(EditInvoiceViewModel editInvoiceViewModel, ILogger<EditInvoice> logger)
	{
		InitializeComponent();
		this._editInvoiceVm = editInvoiceViewModel;
		this._logger = logger;
		BindingContext = this._editInvoiceVm;
	}

	private void customersAutoCompleteEdit_SelectionChanged(object sender, EventArgs e)
	{
		// ReSharper disable once ConditionIsAlwaysTrueOrFalse
		if (this._editInvoiceVm.SelectedCustomer is not null)
		{
			this._editInvoiceVm.CustomersAutoCompleteEditErrorText = string.Empty;
			this._editInvoiceVm.HasCustomersAutoCompleteEditErrors = false;
		}
	}

	private void OpenAddOrEditInvoiceItemBottomSheet(object sender, EventArgs e)
	{
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddOrEditInvoiceItemBottomSheet)} started.");
		if (sender.GetType() == typeof(ImageButton))
		{
			this._editInvoiceVm.InvoiceItem = new InvoiceItem();
			this._editInvoiceVm.InvoiceItemMode = InvoiceItemMode.New;
		}
		else
		{
			var item = (InvoiceItem)((SwipeItemView)sender).CommandParameter;
			this._editInvoiceVm.InvoiceItem = new InvoiceItem
			{
				Id = item.Id,
				ProductId = item.ProductId,
				Quantity = item.Quantity,
				UnitPrice = item.UnitPrice,
				Product = item.Product
			};
			this._editInvoiceVm.InvoiceItemMode = InvoiceItemMode.Edit;
		}
		this._editInvoiceVm.LoadProducts().GetAwaiter().GetResult();
		addItemsBottomSheet.State = BottomSheetState.HalfExpanded;
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(OpenAddOrEditInvoiceItemBottomSheet)} completed.");
	}

	private void EditInvoiceItem_CancelButtonPressed(object sender, EventArgs e)
	{
		addItemsBottomSheet.State = BottomSheetState.Hidden;
		// Clear Invoice Item fields
		this._editInvoiceVm.InvoiceItem = null!;
		this._editInvoiceVm.InvoiceItemMode = InvoiceItemMode.New;
	}

	private void EditInvoiceItem_AddButtonPressed(object sender, EventArgs e)
	{
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(EditInvoiceItem_AddButtonPressed)} started.");
		this._editInvoiceVm.AddOrEditItem().GetAwaiter().GetResult();
		addItemsBottomSheet.State = BottomSheetState.Hidden;
		_logger.LogInformation($"{nameof(EditInvoice)}-{nameof(EditInvoiceItem_AddButtonPressed)} completed.");
	}
}