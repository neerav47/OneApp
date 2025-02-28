using System.Runtime.CompilerServices;
using OneApp.Contracts.v1.Response;
using static OneApp.Constants.Enums;

namespace OneApp.CustomControls;

public partial class EditInvoiceItem : FlexLayout
{
    public EditInvoiceItem()
    {
        InitializeComponent();
    }
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(InvoiceItemMode))
        {
            if (this.InvoiceItemMode == InvoiceItemMode.New)
            {
                this.TitleLabel.Text = "Add Item";
                this.AddButton.Content = "Add";
            }
            else
            {
                this.TitleLabel.Text = "Edit Item";
                this.AddButton.Content = "Save";
            }
        }
    }

    private static readonly BindableProperty ItemAutoCompleteEditErrorTextProperty = BindableProperty.Create(
        propertyName: nameof(ItemAutoCompleteEditErrorText),
        returnType: typeof(string),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public string ItemAutoCompleteEditErrorText
    {
        get { return (string)GetValue(ItemAutoCompleteEditErrorTextProperty); }
        set { SetValue(ItemAutoCompleteEditErrorTextProperty, value); }
    }
    private static readonly BindableProperty HasItemAutoCompleteEditErrorsProperty = BindableProperty.Create(
        propertyName: nameof(HasItemAutoCompleteEditErrors),
        returnType: typeof(bool),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public bool HasItemAutoCompleteEditErrors
    {
        get { return (bool)GetValue(HasItemAutoCompleteEditErrorsProperty); }
        set { SetValue(HasItemAutoCompleteEditErrorsProperty, value); }
    }

    private static readonly BindableProperty UnitPriceTextEditErrorTextProperty = BindableProperty.Create(
        propertyName: nameof(UnitPriceTextEditErrorText),
        returnType: typeof(string),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public string UnitPriceTextEditErrorText
    {
        get { return (string)GetValue(UnitPriceTextEditErrorTextProperty); }
        set { SetValue(UnitPriceTextEditErrorTextProperty, value); }
    }
    private static readonly BindableProperty HasUnitPriceTextEditErrorsProperty = BindableProperty.Create(
        propertyName: nameof(HasUnitPriceTextEditErrors),
        returnType: typeof(bool),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public bool HasUnitPriceTextEditErrors
    {
        get { return (bool)GetValue(HasUnitPriceTextEditErrorsProperty); }
        set { SetValue(HasUnitPriceTextEditErrorsProperty, value); }
    }

    private static readonly BindableProperty QuantityTextEditErrorTextProperty = BindableProperty.Create(
        propertyName: nameof(QuantityTextEditErrorText),
        returnType: typeof(string),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public string QuantityTextEditErrorText
    {
        get { return (string)GetValue(QuantityTextEditErrorTextProperty); }
        set { SetValue(QuantityTextEditErrorTextProperty, value); }
    }

    private static readonly BindableProperty HasQuantityTextEditErrorsProperty = BindableProperty.Create(
        propertyName: nameof(HasQuantityTextEditErrors),
        returnType: typeof(bool),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public bool HasQuantityTextEditErrors
    {
        get { return (bool)GetValue(HasQuantityTextEditErrorsProperty); }
        set { SetValue(HasQuantityTextEditErrorsProperty, value); }
    }

    public static readonly BindableProperty ProductsProperty = BindableProperty.Create(
        propertyName: nameof(Products),
        returnType: typeof(IEnumerable<Product>),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public IEnumerable<Product> Products
    {
        get => (IEnumerable<Product>)GetValue(ProductsProperty);
        set => SetValue(ProductsProperty, value);
    }

    public static readonly BindableProperty InvoiceItemProperty = BindableProperty.Create(
        propertyName: nameof(InvoiceItem),
        returnType: typeof(InvoiceItem),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public InvoiceItem InvoiceItem
    {
        get => (InvoiceItem)GetValue(InvoiceItemProperty);
        set => SetValue(InvoiceItemProperty, value);
    }

    public static readonly BindableProperty InvoiceItemModeProperty = BindableProperty.Create(
        propertyName: nameof(InvoiceItemMode),
        returnType: typeof(InvoiceItemMode),
        declaringType: typeof(EditInvoiceItem),
        defaultValue: InvoiceItemMode.New,
        defaultBindingMode: BindingMode.TwoWay);

    public InvoiceItemMode InvoiceItemMode
    {
        get => (InvoiceItemMode)GetValue(InvoiceItemModeProperty);
        set
        {
            SetValue(InvoiceItemModeProperty, value);
        }
    }

    private static readonly BindableProperty SubTotalProperty = BindableProperty.Create(
        propertyName: nameof(SubTotal),
        returnType: typeof(decimal),
        declaringType: typeof(EditInvoiceItem),
        defaultBindingMode: BindingMode.TwoWay
    );

    public decimal SubTotal
    {
        get { return (decimal)GetValue(SubTotalProperty); }
        set { SetValue(SubTotalProperty, value); }
    }



    #region Events

    public event EventHandler CancelButtonPressed;

    void ItemCancelButtonClicked(object sender, System.EventArgs e)
    {
        this.ClearAddItemsBottomSheetFields();
        CancelButtonPressed?.Invoke(sender, e);
    }

    public event EventHandler AddButtonPressed;

    void AddItemButtonClicked(object sender, System.EventArgs e)
    {
        if (this.Validate())
        {
            return;
        }
        AddButtonPressed?.Invoke(sender, e);
    }
    #endregion

    private void ItemAutoCompleteEdit_SelectionChanged(object sender, EventArgs e)
    {
        if (this.InvoiceItem?.Product is not null)
        {
            this.ItemAutoCompleteEditErrorText = string.Empty;
            this.HasItemAutoCompleteEditErrors = false;
        }
    }

    private void UnitPriceTextEdit_ValueChanged(object sender, EventArgs e)
    {
        if (this.InvoiceItem?.UnitPrice > 0)
        {
            this.UnitPriceTextEditErrorText = string.Empty;
            this.HasUnitPriceTextEditErrors = false;
            // Calculate sub total
            CalculateSubTotal();
        }
    }

    private void QuantityTextEdit_ValueChanged(object sender, EventArgs e)
    {
        if (this.InvoiceItem?.Quantity > 0)
        {
            this.QuantityTextEditErrorText = string.Empty;
            this.HasQuantityTextEditErrors = false;
            // Calculate sub total
            CalculateSubTotal();
        }
    }

    private void CalculateSubTotal()
    {
        this.SubTotal = this.InvoiceItem?.UnitPrice * this.InvoiceItem?.Quantity ?? 0m;
    }

    public void ClearAddItemsBottomSheetFields()
    {
        // Items Edit field
        this.InvoiceItem.Product = null;
        ItemAutoCompleteEditErrorText = string.Empty;
        HasItemAutoCompleteEditErrors = false;

        // Unit price field
        this.InvoiceItem.UnitPrice = 0;
        UnitPriceTextEditErrorText = string.Empty;
        HasUnitPriceTextEditErrors = false;

        // Quantity
        this.InvoiceItem.Quantity = 0;
        QuantityTextEditErrorText = string.Empty;
        HasQuantityTextEditErrors = false;

        // Subtotal
        SubTotal = 0;
    }

    public bool Validate()
    {
        bool hasErrors = false;
        // Validate Selected Item
        if (this.InvoiceItem.Product is null)
        {
            ItemAutoCompleteEditErrorText = "Please select an item";
            HasItemAutoCompleteEditErrors = true;
            hasErrors = true;
        }
        // Validate UnitPrice
        if (this.InvoiceItem.UnitPrice == 0)
        {
            UnitPriceTextEditErrorText = "Unit price must be greater than 0";
            HasUnitPriceTextEditErrors = true;
            hasErrors = true;
        }
        // Validate Quantity
        if (this.InvoiceItem.Quantity == 0)
        {
            QuantityTextEditErrorText = "Quantity must be greater that 0";
            HasQuantityTextEditErrors = true;
            hasErrors = true;
        }
        return hasErrors;
    }
}