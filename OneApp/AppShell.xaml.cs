using OneApp.Views;
using OneApp.Views.Invoice;

namespace OneApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProductDetails), typeof(ProductDetails));
            Routing.RegisterRoute(nameof(InvoiceDetails), typeof(InvoiceDetails));
            Routing.RegisterRoute(nameof(CreateInvoice), typeof(CreateInvoice));
            Routing.RegisterRoute(nameof(EditInvoice), typeof(EditInvoice));
        }
    }
}