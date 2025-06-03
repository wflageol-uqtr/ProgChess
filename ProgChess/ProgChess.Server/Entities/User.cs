using Microsoft.AspNetCore.Identity;

namespace ProChess.Server.Entities;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiry { get; set; }

}