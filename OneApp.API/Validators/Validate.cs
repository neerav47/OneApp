using FluentValidation;
using OneApp.Business.Interfaces;
using OneApp.Contracts.v1;

namespace OneApp.API.Validators;

public class Validate : AbstractValidator<UserRegisterRequest>
{
	private readonly IUserService _userService;
	public Validate(IUserService userService)
	{
		_userService = userService;
		RuleFor(u => u.FirstName)
			.NotEmpty()
			.NotNull()
			.WithMessage("FirstName cannot be null or empty.");
		RuleFor(u => u.LastName)
			.NotEmpty()
			.NotNull()
			.WithMessage("LastName cannot be null or empty.");
		RuleFor(u => u.Email)
			.EmailAddress()
			.WithMessage("Invalid Email format.")
			.NotNull()
			.NotEmpty()
			.WithMessage("Email cannot be null or empty.")
			.MustAsync(async (email, _) =>
			{
				return await _userService.IsEmailUnique(email);
			})
			.WithMessage("Email must be unique.");
		RuleFor(u => u.Password)
			.NotEmpty()
			.NotNull()
			.WithMessage("Password cannot be null or empty.");
		RuleFor(u => u.TenantId)
			.NotEmpty()
			.NotNull()
			.WithMessage("TenantId cannot be null or empty");
	}
}

public class CreateTenantRequestValidator: AbstractValidator<CreateTenantRequest>
{
	private readonly IConfigurationService _configurationService;
    public CreateTenantRequestValidator(IConfigurationService configurationService)
    {
		_configurationService = configurationService;
		RuleFor(t => t.Name)
			.NotNull()
			.NotEmpty()
			.WithMessage("Name cannot be null or empty.")
			.MustAsync(async (name, _) =>
			{
				return await _configurationService.IsTenantNameUnique(name);
			})
			.WithMessage("Name must be unique.");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
		RuleFor(u => u.UserName)
			.NotEmpty()
			.NotNull()
			.WithMessage("UserName cannot be null or empty.");

		RuleFor(u => u.Password)
			.NotEmpty()
			.NotNull()
			.WithMessage("Password cannot be null or empty.");

		RuleFor(u => u.TenantId)
			.NotEmpty()
			.NotNull()
			.WithMessage("TenantId cannot be null or empty.");
    }
}