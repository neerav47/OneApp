using OneApp.Contracts.v1.Enums;

namespace OneApp.Contracts.v1.Response;

public class Invoice
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public long? TransactionId { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public Guid LastUpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public Customer Customer { get; set; } = default!;
    public IEnumerable<InvoiceItem> InvoiceItems { get; set; } = default!;
    public int ItemsCount => InvoiceItems?.Count() ?? 0;
    public decimal Total => InvoiceItems?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;
}

