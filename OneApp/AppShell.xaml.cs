using OneApp.Views;
using OneApp.Views.Category;
using OneApp.Views.Invoice;
using OneApp.Views.Unit;
using OneApp.Views.User;

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
            Routing.RegisterRoute(nameof(Users), typeof(Users));
            Routing.RegisterRoute(nameof(Categories), typeof(Categories));
            Routing.RegisterRoute(nameof(Units),  typeof(Units));
        }
    }
}