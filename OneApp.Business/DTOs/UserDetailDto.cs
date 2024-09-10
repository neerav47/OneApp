
namespace OneApp.Business.DTOs;

public class UserDetailDto
{
    public string Id { get; set; } = default!;
    public TenantDto Tenant { get; set; } = default!;
    public IEnumerable<RoleDto> Roles { get; set; } = default!;
}
