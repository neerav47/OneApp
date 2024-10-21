using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Response;

namespace OneApp.Extentions;

public class UserContext
{
    public User User { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string Name => $"{this.User?.FirstName} {this.User?.LastName}";
}
