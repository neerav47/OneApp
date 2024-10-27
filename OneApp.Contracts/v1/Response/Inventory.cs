using System;
namespace OneApp.Contracts.v1.Response;

public record Inventory(
    int Quantity,
    long TransactionId,
    DateTime CreatedDate,
    Guid CreatedBy,
    DateTime LastUpdatedDate,
    Guid LastUpdatedBy
);

