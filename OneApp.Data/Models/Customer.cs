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
    public string? Email { get; set; }

    [Phone]
    [Required]
    public string Phone { get; set; } = default!;

    [Required]
    public string Address { get; set; } = default!;
}
