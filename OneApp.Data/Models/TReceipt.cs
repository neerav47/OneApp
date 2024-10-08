using Microsoft.EntityFrameworkCore;
using OneApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

[Index(nameof(CustomerId), Name = "IX_CustomerId")]
public sealed class TReceipt
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public long? TransactionId { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public Guid LastUpdatedBy { get; set; }

    public Customer Customer { get; set; } = default!;

    public ICollection<TSaleItem> SaleItems { get; set; } = default!;
}
