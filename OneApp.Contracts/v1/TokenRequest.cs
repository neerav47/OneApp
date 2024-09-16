using OneApp.Contracts.v1.Enums;

namespace OneApp.Contracts.v1;

public class TokenRequest : Token
{
    public TokenType Type { get; set; }
}
     