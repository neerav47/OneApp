using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

public class InventoryHistory
{
    [Required]
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid InvetoryId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public long TransactionId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; }

    [Required]
    public Guid TenantId { get; set; }
}
