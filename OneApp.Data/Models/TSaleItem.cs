using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

[Index(nameof(ReceiptId), Name = "IX_ReceiptId")]
public sealed class TSaleItem
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ReceiptId { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public int UnitPrice { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; }

    public TReceipt Receipt { get; set; } = default!;

    public Product Product { get; set; } = default!;
}
