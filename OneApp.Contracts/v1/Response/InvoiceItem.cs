using System.Security.Cryptography.X509Certificates;

namespace OneApp.Contracts.v1.Response;

public class InvoiceItem
{
    public Guid Id { get; set; }
    public Guid ReceiptId { get; set; }
    public Guid TenantId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public int UnitPrice { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public Guid LastUpdatedBy { get; set; }
    public Product Product { get; set; } = default!;
    public decimal Total => Quantity * UnitPrice;
}