
namespace OneApp.Business.DTOs;

public class UserDetailDto : UserDto
{
    public TenantDto Tenant { get; set; } = default!;
    public IEnumerable<RoleDto> Roles { get; set; } = default!;
    protected internal string[] RoleNames => Roles.Select(r => r.Name).ToArray();
}
