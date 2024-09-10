using OneApp.Business.DTOs;
using OneApp.Contracts.v1;

namespace OneApp.Business.Interfaces;

public interface IUserService
{
    Task<Guid> RegisterUser(UserRegisterRequest request);

    Task<UserDto?> GetUserById(string id);

    Task<IEnumerable<UserDto>> GetAllUsers(string tenantId);

    Task<bool> IsEmailUnique(string email);

    Task<bool> UpdateUserRoles(UserRolesRequest request);

    Task<UserDetailDto?> GetUserByEmail(string email);
}


