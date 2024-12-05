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
    public Guid TenantId { get; set; } = default!;

    public ProductType ProductType { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    public Inventory Inventory { get; set; } = default!;

    public ICollection<InventoryHistory> InventoryHistory { get; set; } = default!;

    public ICollection<TSaleItem> InvoiceItems { get; set; } = default!;

    //public double? UnitPrice { get; set; }
}


