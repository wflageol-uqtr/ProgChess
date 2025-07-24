using Microsoft.AspNetCore.Identity;
using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class User : IdentityUser, ISoftDeletable
{
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiry { get; set; }
    
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    
    public ICollection<Image> Images { get; set; } = new List<Image>();
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}