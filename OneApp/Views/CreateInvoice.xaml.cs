using DevExpress.Maui.Controls;
using DevExpress.Maui.Editors;
using Microsoft.Extensions.Logging;
using OneApp.ViewModels;

namespace OneApp.Views;

public partial class CreateInvoice : ContentPage
{
    private readonly CreateInvoiceViewModel _createInvoiceVM;
    private readonly ILogger<CreateInvoice> _logger;
    public CreateInvoice(
        CreateInvoiceViewModel createInvoiceVM,
        ILogger<CreateInvoice> logger)
    {
        InitializeComponent();
        this._createInvoiceVM = createInvoiceVM;
        this._logger = logger;
        BindingContext = _createInvoiceVM;
        Shell.SetTabBarIsVisible(this, false);
    }

    void OpenAddItemsBottomSheet(System.Object sender, EventArgs e)
    {
        _logger.LogInformation($"{nameof(CreateInvoice)}-{nameof(OpenAddItemsBottomSheet)} started.");
        Task.Run(async () =>
        {
            await this._createInvoiceVM.LoadProducts();
            _logger.LogInformation("Loading products complete.");
        });
        addItemsBottomSheet.State = BottomSheetState.HalfExpanded;
        _logger.LogInformation($"{nameof(CreateInvoice)}-{nameof(OpenAddItemsBottomSheet)} completed.");
    }

    void itemCancelButtonClicked(System.Object sender, System.EventArgs e)
    {
        itemAutoCompleteEdit.Text = string.Empty;
        this._createInvoiceVM.ClearAddItemsBottomSheetFields();
        addItemsBottomSheet.State = BottomSheetState.Hidden;
    }

    void addItemButtonClicked(Object sender, EventArgs e)
    {
        if (this._createInvoiceVM.AddItem())
        {
            return;
        }
        itemAutoCompleteEdit.Text = string.Empty;
        this._createInvoiceVM.ClearAddItemsBottomSheetFields();
        this._createInvoiceVM.CalculateTotal();
        addItemsBottomSheet.State = BottomSheetState.Hidden;
    }

    private void customersAutoCompleteEdit_SelectionChanged(object sender, EventArgs e)
    {
        if (this._createInvoiceVM.SelectedCustomer is not null)
        {
            this._createInvoiceVM.CustomersAutoCompleteEditErrorText = string.Empty;
            this._createInvoiceVM.HasCustomersAutoCompleteEditErrors = false;
        }
    }

    private void itemAutoCompleteEdit_SelectionChanged(object sender, EventArgs e)
    {
        if (this._createInvoiceVM.SelectedProduct is not null)
        {
            this._createInvoiceVM.ItemAutoCompleteEditErrorText = string.Empty;
            this._createInvoiceVM.HasItemAutoCompleteEditErrors = false;
        }
    }

    private void invoiceCancelButtonCLicked(object sender, EventArgs e)
    {
        this._createInvoiceVM.SelectedCustomer = null;
        this._createInvoiceVM.CustomersAutoCompleteEditErrorText = string.Empty;
        this._createInvoiceVM.HasCustomersAutoCompleteEditErrors = false;
        this._createInvoiceVM.NewInvoiceItems = [];
        this._createInvoiceVM.CalculateTotal();
        Shell.Current.GoToAsync("..", true);
    }

    private void unitPriceTextEdit_ValueChanged(object sender, EventArgs e)
    {
        if (this._createInvoiceVM.UnitPrice > 0)
        {
            this._createInvoiceVM.UnitPriceTextEditErrorText = string.Empty;
            this._createInvoiceVM.HasUnitPriceTextEditErrors = false;
            // Calculate sub total
            CalculateSubTotal();
        }
    }

    private void quantityTextEdit_ValueChanged(object sender, EventArgs e)
    {
        if (this._createInvoiceVM.Quantity > 0)
        {
            this._createInvoiceVM.QuantityTextEditErrorText = string.Empty;
            this._createInvoiceVM.HasQuantityTextEditErrors = false;
            // Calculate sub total
            CalculateSubTotal();
        }
    }



    private void CalculateSubTotal()
    {
        this._createInvoiceVM.SubTotal = this._createInvoiceVM.UnitPrice * this._createInvoiceVM.Quantity;
    }
}