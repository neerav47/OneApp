namespace OneApp.Contracts.v1.Response;

public record ProductType(
    Guid Id,
    string Name,
    DateTime CreatedDate,
    string CreatedBy,
    DateTime LastUpdatedDate,
    string LastUpdatedBy);

