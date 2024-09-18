using System.ComponentModel.DataAnnotations;
namespace OneApp.Data.Models;

public class ProductType
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string TenantId { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public string CreatedBy { get; set; } = default!;

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public string LastUpdatedBy { get; set; } = default!;

    public ICollection<Product> Products { get; set; } = default!;
}