using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

public sealed class Customer
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    [Required]
    public string LastName { get; set; } = default!;

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    [Required]
    public string Phone { get; set; } = default!;

    [Required]
    public string Address { get; set; } = default!;

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

    [Required]
    public Guid TenantId { get; set; }
}
