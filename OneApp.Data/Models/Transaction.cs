using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;
public class Transaction
{
    [Required]
    [Key]
    public Guid Id { get; set; }

    [Required]
    public long Number { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime CreatedBy { get; set; }

    [Required]
    public DateTime LastUpdatedDate { get; set; }

    [Required]
    public DateTime LastUpdatedBy { get; set; }
}