namespace OneApp.Business.DTOs;

public sealed class TenantDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}