namespace OneApp.Contracts.v1.Response;

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string FullName => $"{LastName}, {FirstName}";
}