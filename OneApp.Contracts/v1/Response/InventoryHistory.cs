﻿namespace OneApp.Contracts.v1.Response;

public record InventoryHistory(
    Guid InvetoryId,
    Guid ProductId,
    int Quantity,
    long TransactionId,
    DateTime CreatedDate,
    Guid CreatedBy,
    DateTime LastUpdatedDate,
    Guid LastUpdatedBy);
