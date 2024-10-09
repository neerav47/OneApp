namespace OneApp.Business.DTOs;

public class InvoiceItemDto
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
}
