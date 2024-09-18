namespace OneApp.Business.DTOs;

public class ProductTypeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime LastUpdatedDate { get; set; }

    public string LastUpdatedBy { get; set; } = default!;
}