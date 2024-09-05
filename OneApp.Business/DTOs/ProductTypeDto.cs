namespace OneApp.Business.DTOs;

public class ProductTypeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime CreatedDate { get; set; }

    public DateTime CreatedBy { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public DateTime LastUpdatedBy { get; set; }
}