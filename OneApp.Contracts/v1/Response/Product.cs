using System;
namespace OneApp.Contracts.v1.Response;

public record Product(
    Guid Id,
    string Name,
    string Description,
    ProductType ProductType,
    DateTime CreatedDate,
    Guid CreatedBy,
    DateTime LastUpdatedDate,
    Guid LastUpdatedBy,
    Inventory Inventory,
    IEnumerable<InventoryHistory> InventoryHistory,
    IEnumerable<Invoice> Invoices,
    double? AverageSalePrice,
    decimal? TotalUnitSales);