using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

[Index(nameof(Id), nameof(ProductId), IsUnique = true, Name = "IX_Id_ProductId_Unique")]
[Index(nameof(TransactionId), IsUnique = true, Name = "IX_TransactionId_Unique")]
public class Inventory
{
    [Required]
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = default!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public Guid TransactionId { get;set; }

    public Transaction Transaction { get; set; } = default!;

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public DateTime LastUpdatedBy { get; set; }
}
