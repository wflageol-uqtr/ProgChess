using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ResetPasswordDto
{
    [Required]
    public required string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "\nLes mots de passe ne correspondent pas")]
    public string Confirmation { get; set; } = string.Empty;
    
    public string Token { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
}