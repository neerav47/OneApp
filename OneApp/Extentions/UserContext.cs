using OneApp.Contracts.v1;
using OneApp.Contracts.v1.Response;

namespace OneApp.Extentions;

public class UserContext : Token
{
    public User User { get; set; }

    public string Name => $"{this.User?.FirstName} {this.User?.LastName}";
}
