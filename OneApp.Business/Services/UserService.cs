using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OneApp.Business.DTOs;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;
using OneApp.Data.Context;
using OneApp.Data.Models;
using System.Data;

namespace OneApp.Business.Services;

public class UserService(
    DataContext _context,
    ILogger<UserService> _logger,
    IPasswordHasher _passwordHasher,
    IMapper _mapper) : IUserService
{

    #region Public methods
    public async Task<Guid> RegisterUser(UserRegisterRequest request)
    {
        _logger.LogInformation($"{nameof(RegisterUser)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(RegisterUser)} transaction scope started.");
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId.ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                NormalizedEmail = request.Email,
                NormalizedUserName = request.Email,
                TenantId = request.TenantId,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            transaction.Commit();
            _logger.LogInformation($"{nameof(RegisterUser)} transaction scope completed.");
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to register user");
            transaction.Rollback();
            throw new Exception("Failed to register user");
        }   
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers(string tenantId)
    {
        var users = await _context.Users
            .Where(u => u.TenantId.Equals(Guid.Parse(tenantId)))
            .ToListAsync();

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserById(string id)
    {
        var user = await _context.Users
                         .SingleOrDefaultAsync(u => u.Id.ToLower().Equals(id.ToLower()));
        return _mapper.Map<UserDto?>(user);
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await _context.Users.AnyAsync(u => u.Email!.ToLower().Equals(email.ToLower()));
    }

    public async Task<bool> UpdateUserRoles(UserRolesRequest request)
    {
        _logger.LogInformation($"{nameof(UpdateUserRoles)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(UpdateUserRoles)} transaction scope started.");
            // Remove
            if (request.RolesToRemove.Any())
            {
                var userRolesToRemove = _context.UserRoles.Where(u => u.UserId == request.UserId && request.RolesToRemove.Contains(u.RoleId)).ToList();

                _context.UserRoles.RemoveRange(userRolesToRemove);
                await _context.SaveChangesAsync();
            }

            // Add
            if (request.RolesToAdd.Any())
            {
                await _context.UserRoles.AddRangeAsync(
                    request.RolesToAdd.Select(r => new IdentityUserRole<string>
                    {
                        UserId = request.UserId,
                        RoleId = r
                    }));
                await _context.SaveChangesAsync();
            }

            transaction.Commit();
            _logger.LogInformation($"{nameof(UpdateUserRoles)} transaction scope completed.");
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, $"Failed to update user roles");
            throw new Exception("Failed to update user roles");
        }
    }

    public async Task<UserDetailDto?> GetUserByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogError($"{nameof(UserService)}-{nameof(GetUserByEmail)}: email cannot be null or empty");
            return default!;
        }
        var user = await _context.Users
            .Include(u => u.Tenant)
            .SingleOrDefaultAsync(u => u.Email!.ToLower().Equals(email.ToLower()));

        if (user != null)
        {
            var roles = await (from ur in _context.UserRoles
                               join r in _context.Roles on ur.RoleId equals r.Id
                               where ur.UserId == user.Id
                               select new RoleDto
                               {
                                   Id = ur.RoleId,
                                   Name = r.Name!
                               }).ToListAsync();


            return new UserDetailDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Tenant = _mapper.Map<TenantDto>(user.Tenant),
                Roles = roles
            };
        }

        return default;
    }

    public async Task UpdateUserById(string id, IDictionary<string, object> payload)
    {
        _logger.LogInformation($"{nameof(UpdateUserById)} started.");
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted).GetDbTransaction();
        try
        {
            _logger.LogInformation($"{nameof(UpdateUserById)} transaction scope started.");
            var user = await _context
                             .Users
                             .SingleOrDefaultAsync(u => u.Id.ToLower().Equals(id.ToLower()));

            if (user == null)
            {
                _logger.LogInformation($"{nameof(UpdateUserById)} user not found.");
            }

            foreach(var pair in payload.ToArray())
            {
                user!.GetType().GetProperty(pair.Key)?.SetValue(user, pair.Value);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"{nameof(UpdateUserById)} transaction scope completed.");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to update user");
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    #endregion
}

