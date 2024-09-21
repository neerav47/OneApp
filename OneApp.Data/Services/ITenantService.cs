
namespace OneApp.Data.Services;

public interface ITenantService
{
    Guid? GetTenantId();
    Guid? GetUserId();
}
