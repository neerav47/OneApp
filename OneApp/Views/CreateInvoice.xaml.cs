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
    }

    void OpenAddItemsBottomSheet(System.Object sender, EventArgs e)
    {
        _logger.LogInformation($"{nameof(CreateInvoice)}-{nameof(OpenAddItemsBottomSheet)} started.");
        Task.Run(async () =>
        {
            await Task.WhenAll(this._createInvoiceVM.LoadProducts(), this._createInvoiceVM.LoadCustomers());
            _logger.LogInformation("Loading products, customers complete.");
        });
        addItemsBottomSheet.State = BottomSheetState.HalfExpanded;
        _logger.LogInformation($"{nameof(CreateInvoice)}-{nameof(OpenAddItemsBottomSheet)} completed.");
    }

    void DXButton_Clicked(System.Object sender, System.EventArgs e)
    {
        QuantityTextEdit.ClearValue(TextEdit.TextProperty);
        UnitPriceTextEdit.ClearValue(TextEdit.TextProperty);
        addItemsBottomSheet.State = BottomSheetState.Hidden;
    }
}