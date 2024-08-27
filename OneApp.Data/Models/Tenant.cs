using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace OneApp.Data.Models;
public class Tenant
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(6)]
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;
}
