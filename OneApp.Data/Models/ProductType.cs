using System.ComponentModel.DataAnnotations;
namespace OneApp.Data.Models;

public class ProductType
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public DateTime LastUpdatedBy { get; set; }

    public ICollection<Product> Products { get; set; } = default!;
}