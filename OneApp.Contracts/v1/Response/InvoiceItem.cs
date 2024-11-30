namespace OneApp.Contracts.v1.Response;

public record InvoiceItem(
    Guid Id,
    Guid ReceiptId,
    Guid TenantId,
    Guid ProductId,
    decimal Quantity,
    int UnitPrice,
    bool IsDeleted,
    DateTime CreatedDate,
    Guid CreatedBy,
    DateTime LastUpdatedDate,
    Guid LastUpdatedBy,
    Product Product);
