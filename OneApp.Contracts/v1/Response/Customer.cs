namespace OneApp.Contracts.v1.Response;

public record Customer(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string Phone,
    string Address);