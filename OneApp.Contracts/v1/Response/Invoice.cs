using OneApp.Contracts.v1.Enums;

namespace OneApp.Contracts.v1.Response;

public record Invoice(
    Guid Id,
    Guid TenantId,
    Guid CustomerId,
    long? TransactionId,
    Status Status,
    DateTime CreatedDate,
    Guid CreatedBy,
    DateTime LastUpdatedDate,
    Guid LastUpdatedBy,
    bool IsDeleted,
    Customer Customer,
    IEnumerable<InvoiceItem> InvoiceItems);

