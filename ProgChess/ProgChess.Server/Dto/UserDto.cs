using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class UserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}