using OneApp.Views;

namespace OneApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProductDetails), typeof(ProductDetails));
            Routing.RegisterRoute(nameof(InvoiceDetails), typeof(InvoiceDetails));
        }
    }
}