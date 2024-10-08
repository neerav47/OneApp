using OneApp.Data.Enums;

namespace OneApp.Business.DTOs;

public class InvoiceDto
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

    public CustomerDto Customer { get; set; } = default!;

    public IEnumerable<InvoiceItemDto> InvoiceItems { get; set; } = default!;
}
