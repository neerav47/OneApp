using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    [Required]
    public Guid ProductTypeId { get; set; } = default!;

    [Required]
    public string TenantId { get; set; } = default!;

    public ProductType ProductType { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public DateTime LastUpdatedBy { get; set; }
}


