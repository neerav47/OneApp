using System.ComponentModel.DataAnnotations;
namespace OneApp.Data.Models;

public class ProductType
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public Guid TenantId { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public Guid CreatedBy { get; set; } = default!;

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; } = default!;

    public ICollection<Product> Products { get; set; } = default!;
}