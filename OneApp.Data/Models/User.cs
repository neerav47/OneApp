using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OneApp.Data.Models;

public class User: IdentityUser
{
    [Required]
    [ProtectedPersonalData]
    public string FirstName { get; set; } = default!;

    [Required]
    [ProtectedPersonalData]
    public string LastName { get; set; } = default!;

    [Required]
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;
}