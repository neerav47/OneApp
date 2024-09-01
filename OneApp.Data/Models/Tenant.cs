using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public sealed class Tenant
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(6)]
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;
}
