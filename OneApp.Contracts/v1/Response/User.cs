namespace OneApp.Contracts.v1.Response;

public record User(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    Tenant Tenant);
